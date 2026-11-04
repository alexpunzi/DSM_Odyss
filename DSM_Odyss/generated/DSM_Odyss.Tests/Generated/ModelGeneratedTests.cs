using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Infrastructure;
using DSM_Odyss.Infrastructure.Logging;
using Xunit.Abstractions;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Tests.Generated;

public sealed class ModelGeneratedTests
{
    private readonly ITestOutputHelper _output;

    public ModelGeneratedTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void EstadoViaje_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)EstadoViaje.planificado);
        Assert.Equal(2, (int)EstadoViaje.enCurso);
        Assert.Equal(3, (int)EstadoViaje.finalizado);
    }

    [Fact]
    public void TipoMultimedia_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)TipoMultimedia.foto);
        Assert.Equal(2, (int)TipoMultimedia.video);
    }

    [Fact]
    public void TipoViaje_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)TipoViaje.ocio);
        Assert.Equal(2, (int)TipoViaje.trabajo);
        Assert.Equal(3, (int)TipoViaje.familiar);
        Assert.Equal(4, (int)TipoViaje.otro);
    }

    [Fact]
    public void CanalNotificacion_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)CanalNotificacion.email);
        Assert.Equal(2, (int)CanalNotificacion.sms);
    }

    [Fact]
    public void TipoAviso_Should_Define_Model_Literals()
    {
        Assert.Equal(1, (int)TipoAviso.notificacion);
        Assert.Equal(2, (int)TipoAviso.alarma);
    }

    [Fact]
    public void Cliente_ConRecordatoriosActivos_Filter_Contract()
    {
        // <haria-protected id="test:filter:Cliente:conRecordatoriosActivos:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conRecordatoriosActivos";
        var filterExpression = "entity => entity.RecibirRecordatorios";

        // <haria-protected id="test:filter:Cliente:conRecordatoriosActivos:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Cliente.conRecordatoriosActivos declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Cliente.conRecordatoriosActivos declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Cliente_ConViajesPublicos_Filter_Contract()
    {
        // <haria-protected id="test:filter:Cliente:conViajesPublicos:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conViajesPublicos";
        var filterExpression = "entity => entity.Viajes.Any(viaje => viaje.EsPublico)";

        // <haria-protected id="test:filter:Cliente:conViajesPublicos:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Cliente.conViajesPublicos declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Cliente.conViajesPublicos declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Estadisticas_DelAnyo_Filter_Contract()
    {
        // <haria-protected id="test:filter:Estadisticas:delAnyo:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "delAnyo";
        var filterExpression = "entity => entity.Anyo == p_anyo";

        // <haria-protected id="test:filter:Estadisticas:delAnyo:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Estadisticas.delAnyo declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Estadisticas.delAnyo declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Viaje_ViajesPublicos_Filter_Contract()
    {
        // <haria-protected id="test:filter:Viaje:viajesPublicos:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "viajesPublicos";
        var filterExpression = "entity => entity.EsPublico";

        // <haria-protected id="test:filter:Viaje:viajesPublicos:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Viaje.viajesPublicos declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Viaje.viajesPublicos declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Viaje_ViajesPorEstado_Filter_Contract()
    {
        // <haria-protected id="test:filter:Viaje:viajesPorEstado:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "viajesPorEstado";
        var filterExpression = "entity => entity.Estado == p_estado";

        // <haria-protected id="test:filter:Viaje:viajesPorEstado:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Viaje.viajesPorEstado declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Viaje.viajesPorEstado declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void ParadaDestino_PorViaje_Filter_Contract()
    {
        // <haria-protected id="test:filter:ParadaDestino:porViaje:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "porViaje";
        var filterExpression = "entity => entity.Viaje.Id == p_viaje_oid";

        // <haria-protected id="test:filter:ParadaDestino:porViaje:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "ParadaDestino.porViaje declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "ParadaDestino.porViaje declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void ParadaActividad_Pendientes_Filter_Contract()
    {
        // <haria-protected id="test:filter:ParadaActividad:pendientes:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "pendientes";
        var filterExpression = "entity => !entity.Completada";

        // <haria-protected id="test:filter:ParadaActividad:pendientes:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "ParadaActividad.pendientes declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "ParadaActividad.pendientes declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Ubicacion_ConNombre_Filter_Contract()
    {
        // <haria-protected id="test:filter:Ubicacion:conNombre:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conNombre";
        var filterExpression = "entity => entity.Nombre.Contains(p_nombre)";

        // <haria-protected id="test:filter:Ubicacion:conNombre:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Ubicacion.conNombre declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Ubicacion.conNombre declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Destino_PorPais_Filter_Contract()
    {
        // <haria-protected id="test:filter:Destino:porPais:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "porPais";
        var filterExpression = "entity => entity.Pais.Contains(p_pais)";

        // <haria-protected id="test:filter:Destino:porPais:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Destino.porPais declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Destino.porPais declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Carpeta_ConMultimedia_Filter_Contract()
    {
        // <haria-protected id="test:filter:Carpeta:conMultimedia:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conMultimedia";
        var filterExpression = "entity => entity.Multimedia.Any()";

        // <haria-protected id="test:filter:Carpeta:conMultimedia:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Carpeta.conMultimedia declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Carpeta.conMultimedia declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Multimedia_DelTipo_Filter_Contract()
    {
        // <haria-protected id="test:filter:Multimedia:delTipo:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "delTipo";
        var filterExpression = "entity => entity.Tipo == p_tipo";

        // <haria-protected id="test:filter:Multimedia:delTipo:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Multimedia.delTipo declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Multimedia.delTipo declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Collage_DelAnyo_Filter_Contract()
    {
        // <haria-protected id="test:filter:Collage:delAnyo:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "delAnyo";
        var filterExpression = "entity => entity.FechaCreacion.Year == p_anyo";

        // <haria-protected id="test:filter:Collage:delAnyo:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Collage.delAnyo declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Collage.delAnyo declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Recordatorio_Pendientes_Filter_Contract()
    {
        // <haria-protected id="test:filter:Recordatorio:pendientes:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "pendientes";
        var filterExpression = "entity => !entity.Enviado";

        // <haria-protected id="test:filter:Recordatorio:pendientes:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Recordatorio.pendientes declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Recordatorio.pendientes declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Checklist_ConTareas_Filter_Contract()
    {
        // <haria-protected id="test:filter:Checklist:conTareas:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conTareas";
        var filterExpression = "entity => entity.Tareas.Any()";

        // <haria-protected id="test:filter:Checklist:conTareas:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Checklist.conTareas declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Checklist.conTareas declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Tarea_PorEstado_Filter_Contract()
    {
        // <haria-protected id="test:filter:Tarea:porEstado:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "porEstado";
        var filterExpression = "entity => entity.Completada == p_completada";

        // <haria-protected id="test:filter:Tarea:porEstado:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Tarea.porEstado declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Tarea.porEstado declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    public void Resena_ConPuntuacionMinima_Filter_Contract()
    {
        // <haria-protected id="test:filter:Resena:conPuntuacionMinima:arrange">
        // extra user setup
        // </haria-protected>

        var operationName = "conPuntuacionMinima";
        var filterExpression = "entity => entity.Puntuacion >= p_puntuacion";

        // <haria-protected id="test:filter:Resena:conPuntuacionMinima:assert">
        Ensure(!string.IsNullOrWhiteSpace(operationName), "Resena.conPuntuacionMinima declares the generated operation name.");
        Ensure(!string.IsNullOrWhiteSpace(filterExpression), "Resena.conPuntuacionMinima declares the generated filter expression.");
        // </haria-protected>
    }

    [Fact]
    [Trait("Category", "Database")]
    public async Task Generated_Crud_And_Associations_Should_Work_Against_Database()
    {
        var cfg = CreateNHibernateConfiguration();
        new SchemaUpdate(cfg).Execute(false, true);
        using var loggerFactory = LoggerFactory.Create(logging => logging.SetMinimumLevel(LogLevel.Warning));
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

            var recordatorioService = new RecordatorioService(unitOfWorkFactory);
            var sampleRecordatorio = await recordatorioService.CrearRecordatorioAsync("Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 11), new DateTime(2026, 1, 11, 9, 0, 0), TipoAviso.notificacion, true);
            var sampleRecordatorioReadBack = await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id);
            Ensure(sampleRecordatorioReadBack is not null, "Recordatorio read by OID returns the generated entity.");
            var sampleRecordatorioReadAll = await recordatorioService.LeerTodosRecordatoriosAsync();
            Ensure(sampleRecordatorioReadAll.Count > 0, "Recordatorio read all returns generated data.");
            var sampleRecordatorioPendientesResult = await recordatorioService.PendientesAsync();
            Ensure(sampleRecordatorioPendientesResult is not null, "Recordatorio.pendientes filter executes.");
            await recordatorioService.ModificarRecordatorioAsync(sampleRecordatorio.Id, "Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 11), new DateTime(2026, 1, 11, 9, 0, 0), TipoAviso.notificacion, true);
            Ensure(await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id) is not null, "Recordatorio modify keeps the entity readable.");

            var checklistService = new ChecklistService(unitOfWorkFactory);
            var sampleChecklist = await checklistService.CrearChecklistAsync("Sample Checklist Nombre");
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

            await ubicacionService.BorrarUbicacionAsync(sampleUbicacion.Id);
            await destinoService.BorrarDestinoAsync(sampleDestino.Id);
            await recordatorioService.BorrarRecordatorioAsync(sampleRecordatorio.Id);
            await tareaService.BorrarTareaAsync(sampleTarea.Id);
            await checklistService.BorrarChecklistAsync(sampleChecklist.Id);

            Ensure(await ubicacionService.LeerUbicacionOIDAsync(sampleUbicacion.Id) is null, "Ubicacion test entity cleanup completed.");
            Ensure(await destinoService.LeerDestinoOIDAsync(sampleDestino.Id) is null, "Destino test entity cleanup completed.");
            Ensure(await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id) is null, "Recordatorio test entity cleanup completed.");
            Ensure(await checklistService.LeerChecklistOIDAsync(sampleChecklist.Id) is null, "Checklist test entity cleanup completed.");
            Ensure(await tareaService.LeerTareaOIDAsync(sampleTarea.Id) is null, "Tarea test entity cleanup completed.");
    }

    private static Configuration CreateNHibernateConfiguration()
    {
        var configuration = new Configuration();
        var configPath = Path.Combine(AppContext.BaseDirectory, "hibernate.cfg.xml");
        if (File.Exists(configPath))
        {
            configuration.Configure(configPath);
        }
        else
        {
            configuration.Configure();
        }

        configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, "false");
        configuration.AddAssembly(typeof(UnitOfWorkFactory).Assembly);
        configuration.EnableSqlStatementCapture();
        return configuration;
    }

    private const string SuccessColor = "\u001b[32m";
    private const string ErrorColor = "\u001b[31m";
    private const string ResetColor = "\u001b[0m";

    private void Ensure(bool condition, string message)
    {
        WriteTestMessage(condition, message);
        Assert.True(condition, message);
    }

    private void WriteTestMessage(bool condition, string message)
    {
        var color = condition ? SuccessColor : ErrorColor;
        var status = condition ? "[OK] " : "[FAIL] ";
        _output.WriteLine($"{color}{status}{message}{ResetColor}");
    }

}
