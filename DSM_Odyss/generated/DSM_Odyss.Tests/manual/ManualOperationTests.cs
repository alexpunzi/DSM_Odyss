using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;
using DSM_Odyss.Infrastructure;
using DSM_Odyss.Infrastructure.Logging;
using Xunit.Abstractions;

namespace DSM_Odyss.Tests.Manual;

public sealed class ManualOperationTests
{
    private readonly ITestOutputHelper _output;

    public ManualOperationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Contact_alias_and_album_permissions_are_persisted()
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
            Cliente propietario = CrearCliente("propietario");
            Cliente contacto = CrearCliente("contacto");
            await unitOfWork.ClienteRepository.SaveAsync(propietario);
            await unitOfWork.ClienteRepository.SaveAsync(contacto);

            var clienteService = new ClienteService(unitOfWork);
            await clienteService.AgregarContactoAsync(propietario.Id, contacto.Id, "Compañero");
            await clienteService.AgregarContactoAsync(propietario.Id, contacto.Id, "Amigo");

            ClienteContacto relacion = Assert.Single(await unitOfWork.ClienteContactoRepository.ReadAllAsync());
            Assert.Equal(propietario.Id, relacion.Propietario.Id);
            Assert.Equal(contacto.Id, relacion.Contacto.Id);
            Assert.Equal("Amigo", relacion.Alias);

            Carpeta carpeta = new() { Nombre = "Fotos del viaje" };
            await unitOfWork.CarpetaRepository.SaveAsync(carpeta);
            await new CarpetaService(unitOfWork).CompartirCarpetaAsync(carpeta.Id, contacto.Id);

            Carpeta carpetaGuardada = await unitOfWork.CarpetaRepository.ReadAsync(carpeta.Id)
                ?? throw new InvalidOperationException("The shared folder was not saved.");
            Assert.Contains(carpetaGuardada.CompartidoCon, cliente => cliente.Id == contacto.Id);
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

    private const string SuccessColor = "\u001b[32m";
    private const string ErrorColor = "\u001b[31m";
    private const string ResetColor = "\u001b[0m";

    private void EnsureContractOutput(string operationName, string operationType)
    {
        Ensure(!string.IsNullOrWhiteSpace(operationName), $"{operationName} declares the manual operation name.");
        Ensure(!string.IsNullOrWhiteSpace(operationType), $"{operationName} declares the manual operation type.");
        WriteTestMessage(true, $"{operationName} contract uses operation type {operationType}.");
    }

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
