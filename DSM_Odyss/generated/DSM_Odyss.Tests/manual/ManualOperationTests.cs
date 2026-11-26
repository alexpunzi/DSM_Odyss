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
    public async Task Crear_viaje_lo_asocia_al_cliente_y_lo_persiste()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Cliente primerCliente = CrearCliente("primer-cliente");
            Cliente propietario = CrearCliente("viajero");
            await new ClienteService(unitOfWork).AddClienteDefaultAsync(primerCliente);
            await new ClienteService(unitOfWork).AddClienteDefaultAsync(propietario);

            Viaje viaje = await new ViajeService(unitOfWork).CrearViajeAsync(
                "Viaje de prueba",
                new DateTime(2026, 7, 1),
                new DateTime(2026, 7, 5),
                false,
                500,
                TipoViaje.ocio,
                EstadoViaje.planificado,
                propietario.Id,
                0);

            Cliente clientePersistido = await unitOfWork.ClienteRepository.ReadAsync(propietario.Id)
                ?? throw new InvalidOperationException("The owner was not saved.");
            Viaje viajeDelCliente = Assert.Single(clientePersistido.Viajes);
            Assert.Equal(viaje.Id, viajeDelCliente.Id);

            Cliente primerClientePersistido = await unitOfWork.ClienteRepository.ReadAsync(primerCliente.Id)
                ?? throw new InvalidOperationException("The other client was not saved.");
            Assert.Empty(primerClientePersistido.Viajes);
        });
    }

    [Fact]
    public async Task Crear_carpeta_la_asocia_al_viaje_y_persiste_el_permiso_de_edicion()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Cliente propietario = CrearCliente("propietario-carpeta");
            Cliente colaborador = CrearCliente("colaborador-carpeta");
            var clienteService = new ClienteService(unitOfWork);
            await clienteService.AddClienteDefaultAsync(propietario);
            await clienteService.AddClienteDefaultAsync(colaborador);

            Viaje viaje = await new ViajeService(unitOfWork).CrearViajeAsync(
                "Viaje con recuerdos",
                new DateTime(2026, 8, 1),
                new DateTime(2026, 8, 5),
                false,
                300,
                TipoViaje.ocio,
                EstadoViaje.planificado,
                propietario.Id,
                0);

            Carpeta carpeta = await new CarpetaService(unitOfWork)
                .CrearCarpetaAsync("Recuerdos", viaje.Id);
            carpeta.CompartidoCon.Add(colaborador);
            carpeta.CompartidoConEdicion.Add(colaborador);
            Multimedia multimedia = new()
            {
                Tipo = TipoMultimedia.foto,
                Nombre = "playa.jpg",
                FechaSubida = new DateTime(2026, 8, 2),
                Descripcion = "Día de playa",
                RutaArchivo = "test-photo.jpg"
            };
            carpeta.Multimedia.Add(multimedia);
            await unitOfWork.MultimediaRepository.SaveAsync(multimedia);
            await unitOfWork.CarpetaRepository.SaveAsync(carpeta);

            Viaje viajeGuardado = await unitOfWork.ViajeRepository.ReadAsync(viaje.Id)
                ?? throw new InvalidOperationException("The trip was not saved.");
            Assert.Equal(carpeta.Id, viajeGuardado.Carpetas?.Id);

            Carpeta carpetaGuardada = await unitOfWork.CarpetaRepository.ReadAsync(carpeta.Id)
                ?? throw new InvalidOperationException("The folder was not saved.");
            Assert.Contains(carpetaGuardada.CompartidoCon, cliente => cliente.Id == colaborador.Id);
            Assert.Contains(carpetaGuardada.CompartidoConEdicion, cliente => cliente.Id == colaborador.Id);
            Assert.Equal("test-photo.jpg", Assert.Single(carpetaGuardada.Multimedia).RutaArchivo);
        });
    }

    [Fact]
    public async Task Generar_collage_automatico_lo_asocia_al_viaje_y_actualiza_el_existente()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Cliente propietario = CrearCliente("propietario-collage");
            await new ClienteService(unitOfWork).AddClienteDefaultAsync(propietario);
            Viaje viaje = await new ViajeService(unitOfWork).CrearViajeAsync(
                "Viaje con collage",
                new DateTime(2026, 8, 1),
                new DateTime(2026, 8, 5),
                false,
                300,
                TipoViaje.ocio,
                EstadoViaje.planificado,
                propietario.Id,
                0);

            var service = new CollageService(unitOfWork);
            await service.GenerarCollageAutomaticoAsync(viaje.Id, "data:image/jpeg;base64,Zmlyc3Q=");
            await service.GenerarCollageAutomaticoAsync(viaje.Id, "data:image/jpeg;base64,c2Vjb25k");

            Viaje viajeGuardado = await unitOfWork.ViajeRepository.ReadAsync(viaje.Id)
                ?? throw new InvalidOperationException("The trip was not saved.");
            Collage collage = viajeGuardado.Collages
                ?? throw new InvalidOperationException("The generated collage was not linked.");
            Assert.Equal("data:image/jpeg;base64,c2Vjb25k", collage.UrlImagen);
            Assert.Single(await unitOfWork.CollageRepository.ReadAllAsync());
        });
    }

    [Fact]
    public async Task Crear_tarea_la_asocia_y_persiste_en_su_checklist()
    {
        await WithUnitOfWorkAsync(async unitOfWork =>
        {
            Checklist checklist = new() { Nombre = "Preparativos" };
            await unitOfWork.ChecklistRepository.SaveAsync(checklist);

            Tarea tarea = await new TareaService(unitOfWork)
                .CrearTareaAsync("Reservar vuelos", false, checklist.Id);

            Checklist checklistConTareas = Assert.Single(
                await unitOfWork.ChecklistRepository.ConTareasAsync());
            Tarea tareaPersistida = Assert.Single(checklistConTareas.Tareas);
            Assert.Equal(tarea.Id, tareaPersistida.Id);
            Assert.Equal("Reservar vuelos", tareaPersistida.Descripcion);
        });
    }

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
