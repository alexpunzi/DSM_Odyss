using DSM_Odyss.Application.Services;
using DSM_Odyss.Application.UseCases.Checklist;
using DSM_Odyss.Application.UseCases.Estadisticas;
using DSM_Odyss.Application.UseCases.Viaje;
using DSM_Odyss.Domain.Entities;

using DSM_Odyss.Domain.Enums;
using DSM_Odyss.Infrastructure;
using DSM_Odyss.Infrastructure.Repositories;
using DSM_Odyss.Infrastructure.Security;
using Microsoft.Extensions.Logging;

/*PROTECTED REGION ID(seedDataUsings) ENABLED START*/
// Add custom usings here.
/*PROTECTED REGION END*/

namespace InitializeDB;

public static class SeedData
{
    public static async Task InitializeDataAsync(NHibernate.Cfg.Configuration cfg, ILoggerFactory? loggerFactory = null)
    {
        try
        {
            using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            // TODO: No seed generated for Cliente; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Estadisticas; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Viaje; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship ParadaActividad.Viaje manually; it could not be ordered automatically.
            // TODO: No seed generated for ParadaActividad; one or more New arguments reference classes that were not seeded yet.

            var ubicacionService = new UbicacionService(unitOfWorkFactory);
            var sampleUbicacion = await ubicacionService.CrearUbicacionAsync("Sample Ubicacion Nombre", 6.5, 6.5);
            var sampleUbicacionReadBack = await ubicacionService.LeerUbicacionOIDAsync(sampleUbicacion.Id);
            Ensure(sampleUbicacionReadBack is not null, "Ubicacion read by OID returns the generated entity.");
            var sampleUbicacionReadAll = await ubicacionService.LeerTodasUbicacionesAsync();
            Ensure(sampleUbicacionReadAll.Count > 0, "Ubicacion read all returns generated data.");
            var sampleUbicacionConNombreResult = await ubicacionService.ConNombreAsync("Sample Ubicacion Nombre");
            Ensure(sampleUbicacionConNombreResult is not null, "Ubicacion.conNombre filter executes.");
            await ubicacionService.ModificarUbicacionAsync(sampleUbicacion.Id, "Sample Ubicacion Nombre", 6.5, 6.5);
            Ensure(await ubicacionService.LeerUbicacionOIDAsync(sampleUbicacion.Id) is not null, "Ubicacion modify keeps the entity readable.");

            var destinoService = new DestinoService(unitOfWorkFactory);
            var sampleDestino = await destinoService.CrearDestinoAsync("Sample Destino Pais");
            var sampleDestinoReadBack = await destinoService.LeerDestinoOIDAsync(sampleDestino.Id);
            Ensure(sampleDestinoReadBack is not null, "Destino read by OID returns the generated entity.");
            var sampleDestinoReadAll = await destinoService.LeerTodasDestinosAsync();
            Ensure(sampleDestinoReadAll.Count > 0, "Destino read all returns generated data.");
            var sampleDestinoPorPaisResult = await destinoService.PorPaisAsync("Sample Destino Pais");
            Ensure(sampleDestinoPorPaisResult is not null, "Destino.porPais filter executes.");
            await destinoService.ModificarDestinoAsync(sampleDestino.Id, "Sample Destino Pais");
            Ensure(await destinoService.LeerDestinoOIDAsync(sampleDestino.Id) is not null, "Destino modify keeps the entity readable.");

            // TODO: No seed generated for Carpeta; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Multimedia; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Collage; one or more New arguments reference classes that were not seeded yet.
            // Checklists y recordatorios pertenecen a un cliente: creamos uno de ejemplo.
            var clienteService = new ClienteService(unitOfWorkFactory);
            var sampleCliente = await clienteService.CrearClienteAsync("sample_cliente", "Sample1234!", "Sample", "Cliente", "sample@example.com", new DateTime(2000, 1, 1), CanalNotificacion.email, true);
            var recordatorioService = new RecordatorioService(unitOfWorkFactory);
            var sampleRecordatorio = await recordatorioService.CrearRecordatorioAsync("Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 11), new DateTime(2026, 1, 11, 9, 0, 0), TipoAviso.notificacion, true, sampleCliente.Id);
            var sampleRecordatorioReadBack = await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id);
            Ensure(sampleRecordatorioReadBack is not null, "Recordatorio read by OID returns the generated entity.");
            var sampleRecordatorioReadAll = await recordatorioService.LeerTodosRecordatoriosAsync();
            Ensure(sampleRecordatorioReadAll.Count > 0, "Recordatorio read all returns generated data.");
            var sampleRecordatorioPendientesResult = await recordatorioService.PendientesAsync();
            Ensure(sampleRecordatorioPendientesResult is not null, "Recordatorio.pendientes filter executes.");
            await recordatorioService.ModificarRecordatorioAsync(sampleRecordatorio.Id, "Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 11), new DateTime(2026, 1, 11, 9, 0, 0), TipoAviso.notificacion, true);
            Ensure(await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id) is not null, "Recordatorio modify keeps the entity readable.");

            var checklistService = new ChecklistService(unitOfWorkFactory);
            var sampleChecklist = await checklistService.CrearChecklistAsync("Sample Checklist Nombre", sampleCliente.Id);
            var sampleChecklistReadBack = await checklistService.LeerChecklistOIDAsync(sampleChecklist.Id);
            Ensure(sampleChecklistReadBack is not null, "Checklist read by OID returns the generated entity.");
            var sampleChecklistReadAll = await checklistService.LeerTodosChecklistsAsync();
            Ensure(sampleChecklistReadAll.Count > 0, "Checklist read all returns generated data.");
            var sampleChecklistConTareasResult = await checklistService.ConTareasAsync();
            Ensure(sampleChecklistConTareasResult is not null, "Checklist.conTareas filter executes.");
            await checklistService.ModificarChecklistAsync(sampleChecklist.Id, "Sample Checklist Nombre");
            Ensure(await checklistService.LeerChecklistOIDAsync(sampleChecklist.Id) is not null, "Checklist modify keeps the entity readable.");

            var tareaService = new TareaService(unitOfWorkFactory);
            var sampleTarea = await tareaService.CrearTareaAsync("Sample Tarea Descripcion", true, sampleChecklist.Id);
            var sampleTareaReadBack = await tareaService.LeerTareaOIDAsync(sampleTarea.Id);
            Ensure(sampleTareaReadBack is not null, "Tarea read by OID returns the generated entity.");
            var sampleTareaReadAll = await tareaService.LeerTodasTareasAsync();
            Ensure(sampleTareaReadAll.Count > 0, "Tarea read all returns generated data.");
            var sampleTareaPorEstadoResult = await tareaService.PorEstadoAsync(true);
            Ensure(sampleTareaPorEstadoResult is not null, "Tarea.porEstado filter executes.");
            await tareaService.ModificarTareaAsync(sampleTarea.Id, "Sample Tarea Descripcion", true, sampleChecklist.Id);
            Ensure(await tareaService.LeerTareaOIDAsync(sampleTarea.Id) is not null, "Tarea modify keeps the entity readable.");

            // TODO: No seed generated for Resena; one or more New arguments reference classes that were not seeded yet.

            // TODO: Set required relationship ParadaDestino.Viaje manually; it could not be ordered automatically.
            // TODO: No seed generated for ParadaDestino; one or more New arguments reference classes that were not seeded yet.

            await recordatorioService.AsignarTareaAsync(sampleRecordatorio.Id, sampleTarea.Id);
            Ensure(sampleRecordatorio.Tarea?.Equals(sampleTarea) == true, "Recordatorio.asignarTarea relates Recordatorio and Tarea.");
            await checklistService.AgregarTareaAsync(sampleChecklist.Id, new[] { sampleTarea.Id });
            Ensure(sampleChecklist.Tareas.Contains(sampleTarea), "Checklist.agregarTarea relates Checklist and Tarea.");

            await recordatorioService.DesasignarTareaAsync(sampleRecordatorio.Id, sampleTarea.Id);
            Ensure(sampleRecordatorio.Tarea is null, "Recordatorio.desasignarTarea removes the relation between Recordatorio and Tarea.");

            // Por indicación del profesor, los borrados de limpieza quedan comentados
            // para que la base de datos conserve los datos de inicio.
            // await ubicacionService.BorrarUbicacionAsync(sampleUbicacion.Id);
            // await destinoService.BorrarDestinoAsync(sampleDestino.Id);
            // await recordatorioService.BorrarRecordatorioAsync(sampleRecordatorio.Id);
            // await tareaService.BorrarTareaAsync(sampleTarea.Id);
            // await checklistService.BorrarChecklistAsync(sampleChecklist.Id);
            // await clienteService.BorrarClienteAsync(sampleCliente.Id);

            // Como ya no se borra nada, estas comprobaciones de "tabla vacía" fallarían: también comentadas.
            // Ensure((await ubicacionService.LeerTodasUbicacionesAsync()).Count == 0, "Ubicacion seed cleanup completed.");
            // Ensure((await destinoService.LeerTodasDestinosAsync()).Count == 0, "Destino seed cleanup completed.");
            // Ensure((await recordatorioService.LeerTodosRecordatoriosAsync()).Count == 0, "Recordatorio seed cleanup completed.");
            // Ensure((await checklistService.LeerTodosChecklistsAsync()).Count == 0, "Checklist seed cleanup completed.");
            // Ensure((await tareaService.LeerTodasTareasAsync()).Count == 0, "Tarea seed cleanup completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException ?? ex);
            throw;
        }
    }

    public static async Task InitializeDataManualAsync(NHibernate.Cfg.Configuration cfg, ILoggerFactory? loggerFactory = null)
    {
        try
        {
            using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            /*PROTECTED REGION ID(initializeDataManual) ENABLED START*/

            // =====================================================================
            //  DATOS DE INICIO DE ODYSS
            //  Todos los usuarios tienen la contraseña: Odyss1234!
            // =====================================================================
            await using var uow = unitOfWorkFactory.Create();

            var clienteService = new ClienteService(uow);
            var viajeService = new ViajeService(uow);
            var destinoService = new DestinoService(uow);
            var paradaDestinoService = new ParadaDestinoService(uow);
            var paradaActividadService = new ParadaActividadService(uow);
            var carpetaService = new CarpetaService(uow);
            var resenaService = new ResenaService(uow);
            var checklistService = new ChecklistService(uow);
            var tareaService = new TareaService(uow);
            var recordatorioService = new RecordatorioService(uow);

            // ---------- 1. Usuarios ----------
            const string password = "Odyss1234!";
            var aitana = await clienteService.CrearClienteAsync("aitana", password, "Aitana", "Vasco Bechir", "aitana@odyss.es", new DateTime(2003, 5, 14), CanalNotificacion.email, true);
            var juana = await clienteService.CrearClienteAsync("juana", password, "Juana Valentina", "Echeveste", "juana@odyss.es", new DateTime(2003, 9, 2), CanalNotificacion.email, true);
            var trinity = await clienteService.CrearClienteAsync("trinity", password, "Trinity Darlene", "Larsen Campos", "trinity@odyss.es", new DateTime(2002, 12, 20), CanalNotificacion.sms, true);
            var alex = await clienteService.CrearClienteAsync("alex", password, "Alex", "Punzano Gil", "alex@odyss.es", new DateTime(2003, 2, 8), CanalNotificacion.email, false);
            Ensure((await clienteService.LeerTodosClientesAsync()).Count >= 4, "Seed: usuarios creados.");

            // ---------- 2. Ayudantes ----------
            // Reutiliza un destino si ya existe (mismo país y ciudad).
            var destinosCreados = new Dictionary<string, int>();
            async Task<int> DestinoAsync(string pais, string ciudad)
            {
                var clave = $"{pais}|{ciudad}";
                if (destinosCreados.TryGetValue(clave, out var idExistente)) return idExistente;

                var destino = await destinoService.CrearDestinoAsync(pais);
                destino.Ciudad = ciudad;   // CrearDestino solo recibe el país
                await destinoService.ModifierDestinoDefaultAsync(destino);
                destinosCreados[clave] = destino.Id;
                return destino.Id;
            }

            // Crea un viaje con sus destinos (etapas), actividades y carpeta de fotos.
            async Task<(Viaje Viaje, Carpeta Carpeta)> ViajeAsync(
                Cliente autor, string titulo, DateTime inicio, bool esPublico, float presupuesto,
                TipoViaje tipo, EstadoViaje estado,
                (string Pais, string Ciudad, int Dias)[] etapas,
                (string Nombre, int Dia, string Hora, string? Detalles, int Etapa)[] actividades)
            {
                var fin = inicio.AddDays(etapas.Sum(e => e.Dias));
                var viaje = await viajeService.CrearViajeAsync(titulo, inicio, fin, esPublico, presupuesto, tipo, estado, autor.Id, 0);

                var paradas = new List<ParadaDestino>();
                var llegada = inicio;
                var orden = 1;
                foreach (var etapa in etapas)
                {
                    var salida = llegada.AddDays(etapa.Dias);
                    var destinoId = await DestinoAsync(etapa.Pais, etapa.Ciudad);
                    paradas.Add(await paradaDestinoService.CrearParadaDestinoAsync(orden++, llegada, salida, viaje.Id, destinoId));
                    llegada = salida;
                }

                foreach (var act in actividades)
                {
                    var horario = inicio.Date.AddDays(act.Dia - 1).Add(TimeSpan.Parse(act.Hora));
                    var actividad = await paradaActividadService.CrearParadaActividadAsync(
                        act.Nombre, act.Dia, horario, estado == EstadoViaje.finalizado, viaje.Id);
                    actividad.Detalles = act.Detalles;
                    actividad.ParadaDestino = paradas[act.Etapa];
                    await uow.ParadaActividadRepository.SaveAsync(actividad);
                }

                var carpeta = await carpetaService.CrearCarpetaAsync($"Fotos de {titulo}", viaje.Id);
                return (viaje, carpeta);
            }

            // Crea una reseña y la enlaza a su viaje y a su autor (CrearResena no lo hace).
            async Task ResenaAsync(Cliente autor, Viaje viaje, int puntuacion, string comentario, DateTime fecha)
            {
                var resena = await resenaService.CrearResenaAsync(comentario, fecha, puntuacion, viaje.Id, autor.Id);
                viaje.Resenas.Add(resena);
                autor.Resenas.Add(resena);
                await uow.ViajeRepository.SaveAsync(viaje);
                await uow.ClienteRepository.SaveAsync(autor);
            }

            // ---------- 3. Viajes ----------
            var italia = await ViajeAsync(aitana, "Ruta por Italia", new DateTime(2026, 4, 10), true, 1200f,
                TipoViaje.ocio, EstadoViaje.finalizado,
                new[] { ("Italia", "Roma", 3), ("Italia", "Florencia", 2) },
                new[]
                {
                    ("Coliseo y Foro Romano", 1, "10:00", (string?)"Entradas compradas online", 0),
                    ("Museos Vaticanos", 2, "09:30", (string?)null, 0),
                    ("Galería Uffizi", 4, "11:00", (string?)"Reservar con antelación", 1),
                    ("Atardecer en Piazzale Michelangelo", 5, "19:30", (string?)null, 1)
                });

            var lisboa = await ViajeAsync(aitana, "Escapada a Lisboa", new DateTime(2027, 2, 12), false, 450f,
                TipoViaje.ocio, EstadoViaje.planificado,
                new[] { ("Portugal", "Lisboa", 3) },
                new[]
                {
                    ("Tranvía 28 por Alfama", 1, "11:00", (string?)null, 0),
                    ("Pasteles de Belém", 2, "10:00", (string?)"Ir temprano para evitar la cola", 0)
                });

            var japon = await ViajeAsync(juana, "Japón en primavera", new DateTime(2026, 3, 25), true, 2500f,
                TipoViaje.ocio, EstadoViaje.finalizado,
                new[] { ("Japón", "Tokio", 4), ("Japón", "Kioto", 3) },
                new[]
                {
                    ("Cruce de Shibuya", 1, "18:00", (string?)null, 0),
                    ("Templo Senso-ji", 2, "09:00", (string?)null, 0),
                    ("Fushimi Inari", 5, "08:00", (string?)"Subir hasta arriba", 1),
                    ("Bosque de bambú de Arashiyama", 6, "10:00", (string?)null, 1)
                });

            var valencia = await ViajeAsync(juana, "Fin de semana en Valencia", new DateTime(2026, 6, 19), false, 200f,
                TipoViaje.familiar, EstadoViaje.finalizado,
                new[] { ("España", "Valencia", 2) },
                new[] { ("Paella en la Malvarrosa", 1, "14:00", (string?)null, 0) });

            var francia = await ViajeAsync(trinity, "Interrail por Francia", new DateTime(2025, 7, 1), true, 900f,
                TipoViaje.ocio, EstadoViaje.finalizado,
                new[] { ("Francia", "París", 4), ("Francia", "Lyon", 2) },
                new[]
                {
                    ("Torre Eiffel", 1, "20:00", (string?)null, 0),
                    ("Museo del Louvre", 2, "09:00", (string?)"Entrada por la pirámide", 0),
                    ("Vieux Lyon", 5, "11:00", (string?)null, 1)
                });

            var eeuu = await ViajeAsync(alex, "Costa Oeste de EE. UU.", new DateTime(2026, 7, 15), true, 3200f,
                TipoViaje.ocio, EstadoViaje.finalizado,
                new[] { ("Estados Unidos", "Los Ángeles", 4), ("Estados Unidos", "San Francisco", 3) },
                new[]
                {
                    ("Paseo de la Fama", 1, "12:00", (string?)null, 0),
                    ("Puente Golden Gate en bici", 5, "10:00", (string?)null, 1),
                    ("Alcatraz", 6, "09:00", (string?)"Billetes del ferry reservados", 1)
                });

            var congreso = await ViajeAsync(alex, "Congreso en Berlín", new DateTime(2027, 3, 3), false, 700f,
                TipoViaje.trabajo, EstadoViaje.planificado,
                new[] { ("Alemania", "Berlín", 3) },
                new[] { ("Ponencia", 2, "10:00", (string?)"Llevar el portátil", 0) });

            Ensure((await viajeService.LeerTodosViajesAsync()).Count >= 7, "Seed: viajes creados.");

            // ---------- 4. Reseñas cruzadas (solo en viajes públicos de otros) ----------
            await ResenaAsync(juana, italia.Viaje, 5, "¡Itinerario perfecto! Me lo apunto para el verano.", new DateTime(2026, 5, 2));
            await ResenaAsync(trinity, italia.Viaje, 4, "Muy completo, aunque yo añadiría un día en Pisa.", new DateTime(2026, 5, 10));
            await ResenaAsync(aitana, japon.Viaje, 5, "Qué fotos más bonitas de los cerezos.", new DateTime(2026, 4, 20));
            await ResenaAsync(alex, japon.Viaje, 4, "Muy buen ritmo de viaje.", new DateTime(2026, 4, 22));
            await ResenaAsync(alex, francia.Viaje, 4, "El Interrail es la mejor forma de verlo todo.", new DateTime(2025, 8, 3));
            await ResenaAsync(aitana, eeuu.Viaje, 3, "Mucho coche, pero merece la pena.", new DateTime(2026, 8, 12));
            await ResenaAsync(juana, eeuu.Viaje, 5, "¡San Francisco es una pasada!", new DateTime(2026, 8, 15));

            // ---------- 5. Compartir ----------
            // CU-12: Aitana comparte su viaje privado a Lisboa con Juana.
            await viajeService.CompartirConClienteAsync(lisboa.Viaje.Id, new[] { juana.Id });

            // CU-13: Aitana comparte la carpeta de Italia con Juana (como hace CarpetaController.ActualizarPermiso).
            italia.Carpeta.CompartidoCon.Add(juana);
            juana.CarpetasCompartidas.Add(italia.Carpeta);
            await uow.CarpetaRepository.SaveAsync(italia.Carpeta);

            // ---------- 6. Checklists con tareas ----------
            var equipaje = await checklistService.CrearChecklistAsync("Equipaje Lisboa", aitana.Id);
            await tareaService.CrearTareaAsync("DNI y tarjeta sanitaria", true, equipaje.Id);
            await tareaService.CrearTareaAsync("Cargador del móvil", false, equipaje.Id);
            await tareaService.CrearTareaAsync("Chubasquero", false, equipaje.Id);

            var documentos = await checklistService.CrearChecklistAsync("Documentos Berlín", alex.Id);
            await tareaService.CrearTareaAsync("Inscripción al congreso", true, documentos.Id);
            await tareaService.CrearTareaAsync("Tarjeta de embarque", false, documentos.Id);

            var playa = await checklistService.CrearChecklistAsync("Bolsa de playa", juana.Id);
            await tareaService.CrearTareaAsync("Crema solar", false, playa.Id);
            await tareaService.CrearTareaAsync("Toalla", false, playa.Id);

            // ---------- 7. Recordatorios ----------
            // Uno ya vencido y sin enviar: al abrir "Notificaciones y recordatorios" se envía (CU-15).
            var ayer = DateTime.Today.AddDays(-1);
            await recordatorioService.CrearRecordatorioAsync("Comprar billetes a Lisboa", "Mirar vuelos baratos", ayer, ayer.AddHours(10), TipoAviso.alarma, false, aitana.Id);
            var proximaSemana = DateTime.Today.AddDays(7);
            await recordatorioService.CrearRecordatorioAsync("Renovar el pasaporte", string.Empty, proximaSemana, proximaSemana.AddHours(9), TipoAviso.notificacion, false, aitana.Id);
            await recordatorioService.CrearRecordatorioAsync("Reservar hotel en Berlín", "Cerca del recinto del congreso", proximaSemana, proximaSemana.AddHours(18), TipoAviso.notificacion, false, alex.Id);
            await recordatorioService.CrearRecordatorioAsync("Revisar fotos de Japón", "Elegir las mejores para el collage", ayer, ayer.AddHours(20), TipoAviso.notificacion, true, juana.Id);

            Console.WriteLine("Datos de inicio creados. Usuarios: aitana, juana, trinity, alex (contraseña Odyss1234!).");
            /*PROTECTED REGION END*/
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException ?? ex);
            throw;
        }
    }

    private static void Ensure(bool condition, string message)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = condition ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine((condition ? "[OK] " : "[FAIL] ") + message);
        Console.ForegroundColor = previousColor;

        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}