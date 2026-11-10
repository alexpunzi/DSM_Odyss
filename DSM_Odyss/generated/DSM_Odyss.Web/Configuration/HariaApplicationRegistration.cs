// HARIA-WEB GENERATED sha256:884840f674518af0990b1c698d00cc33fa2c25b99c1ce7494e4fa2691845c83d
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DSM_Odyss.Web.Configuration;

public static class HariaApplicationRegistration
{
    public static IServiceCollection AddHariaApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var connectionString = RequireConnectionString(configuration, environment);
        var nhibernate = new NHibernate.Cfg.Configuration();
        nhibernate.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
        nhibernate.SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect");
        nhibernate.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
        nhibernate.SetProperty("adonet.batch_size", "100");
        nhibernate.SetProperty("show_sql", "true");
        nhibernate.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
        nhibernate.AddAssembly(typeof(DSM_Odyss.Infrastructure.UnitOfWorkFactory).Assembly);
        services.AddSingleton(nhibernate.BuildSessionFactory());
        services.AddSingleton<DSM_Odyss.Application.Abstractions.IHariaPasswordHasher, DSM_Odyss.Infrastructure.Security.HariaPasswordHasher>();
        services.AddSingleton<DSM_Odyss.Application.Abstractions.IHariaTokenGenerator, DSM_Odyss.Infrastructure.Security.HariaTokenGenerator>();
        services.AddScoped<DSM_Odyss.Application.Abstractions.IClienteRepository, DSM_Odyss.Infrastructure.Repositories.ClienteRepository>();
        services.AddSingleton<DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory, DSM_Odyss.Infrastructure.UnitOfWorkFactory>();
        services.AddScoped<DSM_Odyss.Application.Services.EstadisticasService>();
        services.AddScoped<DSM_Odyss.Application.Services.ViajeService>();
        services.AddScoped<DSM_Odyss.Application.Services.ParadaDestinoService>();
        services.AddScoped<DSM_Odyss.Application.Services.ParadaActividadService>();
        services.AddScoped<DSM_Odyss.Application.Services.UbicacionService>();
        services.AddScoped<DSM_Odyss.Application.Services.DestinoService>();
        services.AddScoped<DSM_Odyss.Application.Services.CarpetaService>();
        services.AddScoped<DSM_Odyss.Application.Services.MultimediaService>();
        services.AddScoped<DSM_Odyss.Application.Services.CollageService>();
        services.AddScoped<DSM_Odyss.Application.Services.RecordatorioService>();
        services.AddScoped<DSM_Odyss.Application.Services.ChecklistService>();
        services.AddScoped<DSM_Odyss.Application.Services.TareaService>();
        services.AddScoped<DSM_Odyss.Application.Services.ResenaService>();
        services.AddScoped<DSM_Odyss.Application.Services.ClienteService>();
        return services;
    }

    private static string RequireConnectionString(IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure ConnectionStrings:DbConnection in appsettings.Haria.json, user-secrets, or the ConnectionStrings__DbConnection environment variable.");
        }

        var sqlite = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = connectionString };
        var dataSourceKey = sqlite.Keys.Cast<string>().FirstOrDefault(key =>
            string.Equals(key.Replace(" ", ""), "DataSource", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "Filename", StringComparison.OrdinalIgnoreCase));
        if (dataSourceKey is null)
        {
            throw new InvalidOperationException("The SQLite connection string must specify Data Source or Filename.");
        }
        var dataSource = Convert.ToString(sqlite[dataSourceKey]) ?? string.Empty;
        if (dataSource.Length == 0 || dataSource.StartsWith("file:", StringComparison.OrdinalIgnoreCase) || Path.IsPathRooted(dataSource)
            || string.Equals(dataSource, ":memory:", StringComparison.OrdinalIgnoreCase)
            || (sqlite.TryGetValue("Mode", out var mode) && string.Equals(Convert.ToString(mode), "Memory", StringComparison.OrdinalIgnoreCase)))
        {
            return sqlite.ConnectionString;
        }

        sqlite[dataSourceKey] = Path.GetFullPath(Path.Combine(environment.ContentRootPath, dataSource));
        return sqlite.ConnectionString;
    }
}
