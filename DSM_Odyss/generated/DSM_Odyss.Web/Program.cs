using System.Data.SQLite;
using DSM_Odyss.Infrastructure.Repositories;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

var builder = WebApplication.CreateBuilder(args);
// HARIA-WEB: CONFIGURATION BASE
builder.Configuration.Sources.Insert(0, new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource { Path = "appsettings.Haria.json", Optional = false, ReloadOnChange = true, FileProvider = builder.Environment.ContentRootFileProvider });

EnsureDatabaseSchema(builder.Configuration.GetConnectionString("DbConnection"), builder.Environment.ContentRootPath);

// Add services to the container.
builder.Services.AddControllersWithViews();
// HARIA-WEB: LOCALIZATION SERVICES
DSM_Odyss.Web.Utils.Localization.HariaWebLocalization.AddHariaWebLocalization(builder.Services, builder.Configuration);
// HARIA-WEB: USER SESSION SERVICES
DSM_Odyss.Web.Utils.Security.HariaWebClassSession.AddHariaWebClassSession(builder.Services);
// HARIA-WEB: SECURITY BEGIN
DSM_Odyss.Web.Configuration.HariaApplicationRegistration.AddHariaApplication(builder.Services, builder.Configuration, builder.Environment);
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cookie =>
    {
        cookie.LoginPath = "/Account/Login";
        cookie.AccessDeniedPath = "/Account/AccessDenied";
        cookie.Cookie.HttpOnly = true;
        cookie.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        cookie.Cookie.SameSite = SameSiteMode.Lax;
        cookie.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
// HARIA-WEB: SECURITY END

var app = builder.Build();
// HARIA-WEB: LOCALIZATION MIDDLEWARE
app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// HARIA-WEB: USER SESSION MIDDLEWARE
app.UseSession();
app.UseRouting();

// HARIA-WEB: SECURITY MIDDLEWARE BEGIN
app.UseAuthentication();
// HARIA-WEB: SECURITY MIDDLEWARE END
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static void EnsureDatabaseSchema(string? connectionString, string contentRootPath)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return;
    }

    var sqlite = new System.Data.Common.DbConnectionStringBuilder { ConnectionString = connectionString };
    var dataSourceKey = sqlite.Keys.Cast<string>().FirstOrDefault(key =>
        string.Equals(key.Replace(" ", string.Empty), "DataSource", StringComparison.OrdinalIgnoreCase)
        || string.Equals(key, "Filename", StringComparison.OrdinalIgnoreCase));

    string? databasePath = null;
    if (dataSourceKey is not null)
    {
        var candidate = Convert.ToString(sqlite[dataSourceKey]);
        if (!string.IsNullOrWhiteSpace(candidate) &&
            !candidate.StartsWith("file:", StringComparison.OrdinalIgnoreCase) &&
            !Path.IsPathRooted(candidate) &&
            !string.Equals(candidate, ":memory:", StringComparison.OrdinalIgnoreCase) &&
            !(sqlite.TryGetValue("Mode", out var mode) && string.Equals(Convert.ToString(mode), "Memory", StringComparison.OrdinalIgnoreCase)))
        {
            candidate = Path.GetFullPath(Path.Combine(contentRootPath, candidate));
        }

        databasePath = candidate;
    }

    if (databasePath is not null && File.Exists(databasePath))
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%';";
        var tablesCount = Convert.ToInt32(command.ExecuteScalar());
        if (tablesCount > 0)
        {
            return;
        }
    }

    var configuration = new Configuration();
    configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
    configuration.SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect");
    configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
    configuration.SetProperty("adonet.batch_size", "100");
    configuration.SetProperty("show_sql", "true");
    configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
    configuration.AddAssembly(typeof(DSM_Odyss.Infrastructure.UnitOfWorkFactory).Assembly);

    new SchemaExport(configuration).Execute(false, true, false);
}
