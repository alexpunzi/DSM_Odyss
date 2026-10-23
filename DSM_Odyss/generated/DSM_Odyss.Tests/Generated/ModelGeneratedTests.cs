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
    [Trait("Category", "Database")]
    public async Task Generated_Crud_And_Associations_Should_Work_Against_Database()
    {
        var cfg = CreateNHibernateConfiguration();
        new SchemaUpdate(cfg).Execute(false, true);
        using var loggerFactory = LoggerFactory.Create(logging => logging.SetMinimumLevel(LogLevel.Warning));
        using var unitOfWorkFactory = new UnitOfWorkFactory(cfg, loggerFactory);

            // TODO: No seed generated for Cliente; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Estadisticas; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for ParadaActividad; one or more New arguments reference classes that were not seeded yet.

            var ubicacionService = new UbicacionService(unitOfWorkFactory);
            var sampleUbicacion = await ubicacionService.CrearUbicacionAsync("Sample Ubicacion Nombre", 5.5, 5.5);
            var sampleUbicacionReadBack = await ubicacionService.LeerUbicacionOIDAsync(sampleUbicacion.Id);
            Ensure(sampleUbicacionReadBack is not null, "Ubicacion read by OID returns the generated entity.");
            var sampleUbicacionReadAll = await ubicacionService.LeerTodasUbicacionesAsync();
            Ensure(sampleUbicacionReadAll.Count > 0, "Ubicacion read all returns generated data.");
            await ubicacionService.ModificarUbicacionAsync(sampleUbicacion.Id, "Sample Ubicacion Nombre", 5.5, 5.5);
            Ensure(await ubicacionService.LeerUbicacionOIDAsync(sampleUbicacion.Id) is not null, "Ubicacion modify keeps the entity readable.");

            var destinoService = new DestinoService(unitOfWorkFactory);
            var sampleDestino = await destinoService.CrearDestinoAsync("Sample Destino Pais");
            var sampleDestinoReadBack = await destinoService.LeerDestinoOIDAsync(sampleDestino.Id);
            Ensure(sampleDestinoReadBack is not null, "Destino read by OID returns the generated entity.");
            var sampleDestinoReadAll = await destinoService.LeerTodasDestinosAsync();
            Ensure(sampleDestinoReadAll.Count > 0, "Destino read all returns generated data.");
            await destinoService.ModificarDestinoAsync(sampleDestino.Id, "Sample Destino Pais");
            Ensure(await destinoService.LeerDestinoOIDAsync(sampleDestino.Id) is not null, "Destino modify keeps the entity readable.");

            // TODO: No seed generated for Carpeta; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Multimedia; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Collage; one or more New arguments reference classes that were not seeded yet.

            var recordatorioService = new RecordatorioService(unitOfWorkFactory);
            var sampleRecordatorio = await recordatorioService.CrearRecordatorioAsync("Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 10), new DateTime(2026, 1, 10, 9, 0, 0), TipoAviso.notificacion, true);
            var sampleRecordatorioReadBack = await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id);
            Ensure(sampleRecordatorioReadBack is not null, "Recordatorio read by OID returns the generated entity.");
            var sampleRecordatorioReadAll = await recordatorioService.LeerTodosRecordatoriosAsync();
            Ensure(sampleRecordatorioReadAll.Count > 0, "Recordatorio read all returns generated data.");
            await recordatorioService.ModificarRecordatorioAsync(sampleRecordatorio.Id, "Sample Recordatorio Titulo", "Sample Recordatorio Detalles", new DateTime(2026, 1, 10), new DateTime(2026, 1, 10, 9, 0, 0), TipoAviso.notificacion, true);
            Ensure(await recordatorioService.LeerRecordatorioOIDAsync(sampleRecordatorio.Id) is not null, "Recordatorio modify keeps the entity readable.");

            var checklistService = new ChecklistService(unitOfWorkFactory);
            var sampleChecklist = await checklistService.CrearChecklistAsync("Sample Checklist Nombre");
            var sampleChecklistReadBack = await checklistService.LeerChecklistOIDAsync(sampleChecklist.Id);
            Ensure(sampleChecklistReadBack is not null, "Checklist read by OID returns the generated entity.");
            var sampleChecklistReadAll = await checklistService.LeerTodosChecklistsAsync();
            Ensure(sampleChecklistReadAll.Count > 0, "Checklist read all returns generated data.");
            await checklistService.ModificarChecklistAsync(sampleChecklist.Id, "Sample Checklist Nombre");
            Ensure(await checklistService.LeerChecklistOIDAsync(sampleChecklist.Id) is not null, "Checklist modify keeps the entity readable.");

            var tareaService = new TareaService(unitOfWorkFactory);
            var sampleTarea = await tareaService.CrearTareaAsync("Sample Tarea Descripcion", true, sampleChecklist.Id);
            var sampleTareaReadBack = await tareaService.LeerTareaOIDAsync(sampleTarea.Id);
            Ensure(sampleTareaReadBack is not null, "Tarea read by OID returns the generated entity.");
            var sampleTareaReadAll = await tareaService.LeerTodasTareasAsync();
            Ensure(sampleTareaReadAll.Count > 0, "Tarea read all returns generated data.");
            await tareaService.ModificarTareaAsync(sampleTarea.Id, "Sample Tarea Descripcion", true, sampleChecklist.Id);
            Ensure(await tareaService.LeerTareaOIDAsync(sampleTarea.Id) is not null, "Tarea modify keeps the entity readable.");

            // TODO: No seed generated for Resena; one or more New arguments reference classes that were not seeded yet.

            // TODO: No seed generated for Viaje; one or more New arguments reference classes that were not seeded yet.

            await ubicacionService.BorrarUbicacionAsync(sampleUbicacion.Id);
            await destinoService.BorrarDestinoAsync(sampleDestino.Id);
            await recordatorioService.BorrarRecordatorioAsync(sampleRecordatorio.Id);
            await tareaService.BorrarTareaAsync(sampleTarea.Id);
            await checklistService.BorrarChecklistAsync(sampleChecklist.Id);

            Ensure((await ubicacionService.LeerTodasUbicacionesAsync()).Count == 0, "Ubicacion seed cleanup completed.");
            Ensure((await destinoService.LeerTodasDestinosAsync()).Count == 0, "Destino seed cleanup completed.");
            Ensure((await recordatorioService.LeerTodosRecordatoriosAsync()).Count == 0, "Recordatorio seed cleanup completed.");
            Ensure((await checklistService.LeerTodosChecklistsAsync()).Count == 0, "Checklist seed cleanup completed.");
            Ensure((await tareaService.LeerTodasTareasAsync()).Count == 0, "Tarea seed cleanup completed.");
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
