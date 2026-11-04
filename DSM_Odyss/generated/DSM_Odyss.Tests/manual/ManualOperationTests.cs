using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;
using DSM_Odyss.Application.UseCases.Checklist;
using DSM_Odyss.Application.UseCases.Viaje;
using DSM_Odyss.Infrastructure;
using DSM_Odyss.Infrastructure.Logging;

namespace DSM_Odyss.Tests.Manual;

public sealed class ManualOperationTests
{
    [Fact]
    public async Task Contact_alias_and_album_permissions_are_persisted()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Cliente propietario = CrearCliente("propietario");
            Cliente contacto = CrearCliente("contacto");

            var clienteService = new ClienteService(unitOfWork);
            await clienteService.AddClienteDefaultAsync(propietario);
            await clienteService.AddClienteDefaultAsync(contacto);
            await clienteService.AgregarContactoAsync(propietario.Id, contacto.Id, "Compañero");
            await clienteService.AgregarContactoAsync(propietario.Id, contacto.Id, "Amigo");

            ClienteContacto relacion = Assert.Single(await unitOfWork.ClienteContactoRepository.ReadAllAsync());
            Assert.Equal(propietario.Id, relacion.Propietario.Id);
            Assert.Equal(contacto.Id, relacion.Contacto.Id);
            Assert.Equal("Amigo", relacion.Alias);

            Assert.True(await clienteService.CambiarPasswordAsync(
                propietario.Id,
                "unused-test-password",
                "NewPassword1!"));
            string token = await clienteService.LoginAsync("propietario", "NewPassword1!")
                ?? throw new InvalidOperationException("Login did not return a token.");
            Assert.True(unitOfWork.TokenGenerator.TryValidateToken(token, out var tokenDescriptor));
            Assert.Equal(propietario.Id.ToString(), tokenDescriptor?.Subject);
            Assert.Equal("propietario", tokenDescriptor?.Claims["user"]);
            Assert.Null(await clienteService.LoginAsync("propietario", "wrong-password"));
            await clienteService.ConfigurarNotificacionesAsync(
                propietario.Id,
                CanalNotificacion.sms,
                false);
            Cliente clienteActualizado = await unitOfWork.ClienteRepository.ReadAsync(propietario.Id)
                ?? throw new InvalidOperationException("The client was not saved.");
            Assert.Equal(CanalNotificacion.sms, clienteActualizado.CanalNotificacion);
            Assert.False(clienteActualizado.RecibirRecordatorios);

            Tarea tarea = new() { Descripcion = "Finish itinerary", Completada = false };
            await unitOfWork.TareaRepository.SaveAsync(tarea);
            await new TareaService(unitOfWork).CambiarEstadoTareaAsync(tarea.Id, true);
            Assert.True((await unitOfWork.TareaRepository.ReadAsync(tarea.Id))?.Completada);

            Carpeta carpeta = new() { Nombre = "Fotos del viaje" };
            await unitOfWork.CarpetaRepository.SaveAsync(carpeta);
            await new CarpetaService(unitOfWork).CompartirCarpetaAsync(carpeta.Id, contacto.Id);

            Carpeta carpetaGuardada = await unitOfWork.CarpetaRepository.ReadAsync(carpeta.Id)
                ?? throw new InvalidOperationException("The shared folder was not saved.");
            Assert.Contains(carpetaGuardada.CompartidoCon, cliente => cliente.Id == contacto.Id);
        });
    }

    [Fact]
    public async Task Read_filters_return_only_matching_records()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            await unitOfWork.UbicacionRepository.SaveAsync(new Ubicacion
            {
                Nombre = "Roma centro",
                Latitud = 41.9,
                Longitud = 12.5,
            });
            await unitOfWork.UbicacionRepository.SaveAsync(new Ubicacion
            {
                Nombre = "Paris centro",
                Latitud = 48.8,
                Longitud = 2.3,
            });

            await unitOfWork.DestinoRepository.SaveAsync(new Destino { Pais = "Italia" });
            await unitOfWork.DestinoRepository.SaveAsync(new Destino { Pais = "Francia" });

            Recordatorio pendiente = CrearRecordatorio("Pending", enviado: false);
            Recordatorio enviado = CrearRecordatorio("Sent", enviado: true);
            await unitOfWork.RecordatorioRepository.SaveAsync(pendiente);
            await unitOfWork.RecordatorioRepository.SaveAsync(enviado);

            Tarea pendienteTarea = new() { Descripcion = "Open", Completada = false };
            Tarea completadaTarea = new() { Descripcion = "Done", Completada = true };
            await unitOfWork.TareaRepository.SaveAsync(pendienteTarea);
            await unitOfWork.TareaRepository.SaveAsync(completadaTarea);

            Assert.Equal(
                "Roma centro",
                Assert.Single(await new UbicacionService(unitOfWork).ConNombreAsync("Roma")).Nombre);
            Assert.Equal(
                "Italia",
                Assert.Single(await new DestinoService(unitOfWork).PorPaisAsync("Ital")).Pais);
            Assert.Equal(
                pendiente.Id,
                Assert.Single(await new RecordatorioService(unitOfWork).PendientesAsync()).Id);
            Assert.Equal(
                pendienteTarea.Id,
                Assert.Single(await new TareaService(unitOfWork).PorEstadoAsync(false)).Id);
        });
    }

    [Fact]
    public async Task Custom_transactions_commit_checklist_and_itinerary_changes()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Checklist checklist = new() { Nombre = "Trip tasks" };
            Tarea tarea = new() { Descripcion = "Book tickets", Completada = false };
            await unitOfWork.ChecklistRepository.SaveAsync(checklist);
            await unitOfWork.TareaRepository.SaveAsync(tarea);
            await new ChecklistService(unitOfWork).AgregarTareaAsync(checklist.Id, new[] { tarea.Id });

            await new ChecklistUseCase(unitOfWork).VaciarChecklistAsync(checklist.Id);
            Assert.Null(await unitOfWork.TareaRepository.ReadAsync(tarea.Id));

            Destino madrid = new() { Pais = "Spain", Ciudad = "Madrid" };
            Destino barcelona = new() { Pais = "Spain", Ciudad = "Barcelona" };
            await unitOfWork.DestinoRepository.SaveAsync(madrid);
            await unitOfWork.DestinoRepository.SaveAsync(barcelona);
            Viaje original = new()
            {
                Titulo = "Summer trip",
                FechaInicio = new DateTime(2026, 7, 1),
                FechaFin = new DateTime(2026, 7, 7),
                EsPublico = true,
                Presupuesto = 1200,
                TipoViaje = TipoViaje.ocio,
                Estado = EstadoViaje.enCurso,
            };
            await unitOfWork.ViajeRepository.SaveAsync(original);
            var paradaDestinoService = new ParadaDestinoService(unitOfWork);
            await paradaDestinoService.CrearParadaDestinoAsync(
                2,
                new DateTime(2026, 7, 4),
                new DateTime(2026, 7, 7),
                original.Id,
                barcelona.Id);
            ParadaDestino primeraParada = await paradaDestinoService.CrearParadaDestinoAsync(
                1,
                new DateTime(2026, 7, 1),
                new DateTime(2026, 7, 4),
                original.Id,
                madrid.Id);
            ParadaActividad actividad = await new ParadaActividadService(unitOfWork)
                .CrearParadaActividadAsync(
                    "Visit museum",
                    1,
                    new DateTime(2026, 7, 2, 10, 0, 0),
                    false,
                    original.Id);
            await new ParadaActividadService(unitOfWork)
                .AsignarParadaDestinoAsync(actividad.Id, primeraParada.Id);
            Assert.Equal(2, (await paradaDestinoService.PorViajeAsync(original.Id)).Count);
            await Assert.ThrowsAsync<DSM_Odyss.Application.Exceptions.ModelException>(() =>
                paradaDestinoService.CrearParadaDestinoAsync(
                    1,
                    new DateTime(2026, 7, 1),
                    new DateTime(2026, 7, 2),
                    original.Id,
                    madrid.Id));

            Viaje copia = await new ViajeUseCase(unitOfWork)
                .DuplicarItinerarioAsync(original.Id, "My copy");
            Assert.NotEqual(original.Id, copia.Id);
            Assert.Equal("My copy", copia.Titulo);
            Assert.False(copia.EsPublico);
            Assert.Equal(EstadoViaje.planificado, copia.Estado);
            IReadOnlyList<ParadaDestino> paradasCopia = await paradaDestinoService.PorViajeAsync(copia.Id);
            Assert.Collection(
                paradasCopia,
                primera => Assert.Equal("Madrid", primera.Destino.Ciudad),
                segunda => Assert.Equal("Barcelona", segunda.Destino.Ciudad));
            ParadaActividad actividadCopia = Assert.Single(
                await unitOfWork.ParadaActividadRepository.ReadAllAsync(),
                item => item.Viaje.Id == copia.Id);
            Assert.Equal(paradasCopia[0].Id, actividadCopia.ParadaDestino?.Id);
            Assert.False(actividadCopia.Completada);
        });
    }

    private static async Task WithUnitOfWorkAsync(Func<IUnitOfWork, Task> action)
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"odyss-manual-{Guid.NewGuid():N}.sqlite");
        try
        {
            var configuration = new Configuration();
            configuration.Configure(Path.Combine(AppContext.BaseDirectory, "hibernate.cfg.xml"));
            configuration.SetProperty(
                NHibernate.Cfg.Environment.ConnectionString,
                $"Data Source={databasePath};Version=3;New=True;");
            configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, "false");
            configuration.AddAssembly(typeof(UnitOfWorkFactory).Assembly);
            configuration.EnableSqlStatementCapture();
            new SchemaUpdate(configuration).Execute(false, true);

            using var unitOfWorkFactory = new UnitOfWorkFactory(configuration);
            await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
            await action(unitOfWork);
        }
        finally
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }

    private static Cliente CrearCliente(string user) =>
        new()
        {
            User = user,
            Password = "unused-test-password",
            Nombre = user,
            Apellidos = "Test",
            Email = $"{user}@example.test",
            FechaNacimiento = new DateTime(2000, 1, 1),
            CanalNotificacion = CanalNotificacion.email,
            RecibirRecordatorios = true,
        };

    private static Recordatorio CrearRecordatorio(string titulo, bool enviado = false) =>
        new()
        {
            Titulo = titulo,
            Detalles = "Manual-operation test",
            Fecha = DateTime.UtcNow.Date,
            Hora = DateTime.UtcNow,
            TipoAviso = TipoAviso.notificacion,
            Enviado = enviado,
        };
}
