using NHibernate;
using Microsoft.Extensions.Logging;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Infrastructure.Exceptions;
using DSM_Odyss.Infrastructure.Logging;
using DSM_Odyss.Infrastructure.Repositories;
using DSM_Odyss.Infrastructure.Security;

namespace DSM_Odyss.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly ISession _session;
    private readonly ILogger<UnitOfWork>? _logger;
    private readonly Microsoft.Extensions.Logging.ILoggerFactory? _loggerFactory;
    private ITransaction? _transaction;
    private SqlStatementCaptureScope? _sqlCaptureScope;
    private bool _disposed;

    public UnitOfWork(UnitOfWorkFactory unitOfWorkFactory)
        : this(
            unitOfWorkFactory.OpenSession(),
            unitOfWorkFactory.LoggerFactory,
            unitOfWorkFactory.PasswordHasher,
            unitOfWorkFactory.TokenGenerator)
    {
    }

    public UnitOfWork(ISession session, Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null, IHariaPasswordHasher? passwordHasher = null, IHariaTokenGenerator? tokenGenerator = null)
    {
        _session = session;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory?.CreateLogger<UnitOfWork>();
        PasswordHasher = passwordHasher ?? new HariaPasswordHasher();
        TokenGenerator = tokenGenerator ?? new HariaTokenGenerator();
        ClienteRepository = new ClienteRepository(session, _loggerFactory?.CreateLogger<ClienteRepository>());
        EstadisticasRepository = new EstadisticasRepository(session, _loggerFactory?.CreateLogger<EstadisticasRepository>());
        ViajeRepository = new ViajeRepository(session, _loggerFactory?.CreateLogger<ViajeRepository>());
        ParadaActividadRepository = new ParadaActividadRepository(session, _loggerFactory?.CreateLogger<ParadaActividadRepository>());
        UbicacionRepository = new UbicacionRepository(session, _loggerFactory?.CreateLogger<UbicacionRepository>());
        DestinoRepository = new DestinoRepository(session, _loggerFactory?.CreateLogger<DestinoRepository>());
        CarpetaRepository = new CarpetaRepository(session, _loggerFactory?.CreateLogger<CarpetaRepository>());
        MultimediaRepository = new MultimediaRepository(session, _loggerFactory?.CreateLogger<MultimediaRepository>());
        CollageRepository = new CollageRepository(session, _loggerFactory?.CreateLogger<CollageRepository>());
        RecordatorioRepository = new RecordatorioRepository(session, _loggerFactory?.CreateLogger<RecordatorioRepository>());
        ChecklistRepository = new ChecklistRepository(session, _loggerFactory?.CreateLogger<ChecklistRepository>());
        TareaRepository = new TareaRepository(session, _loggerFactory?.CreateLogger<TareaRepository>());
        ResenaRepository = new ResenaRepository(session, _loggerFactory?.CreateLogger<ResenaRepository>());
    }

    public IClienteRepository ClienteRepository { get; }
    public IEstadisticasRepository EstadisticasRepository { get; }
    public IViajeRepository ViajeRepository { get; }
    public IParadaActividadRepository ParadaActividadRepository { get; }
    public IUbicacionRepository UbicacionRepository { get; }
    public IDestinoRepository DestinoRepository { get; }
    public ICarpetaRepository CarpetaRepository { get; }
    public IMultimediaRepository MultimediaRepository { get; }
    public ICollageRepository CollageRepository { get; }
    public IRecordatorioRepository RecordatorioRepository { get; }
    public IChecklistRepository ChecklistRepository { get; }
    public ITareaRepository TareaRepository { get; }
    public IResenaRepository ResenaRepository { get; }

    public IHariaPasswordHasher PasswordHasher { get; }

    public IHariaTokenGenerator TokenGenerator { get; }

    public Task BeginTransactionAsync(string? businessOperation = null)
    {
        try
        {
            if (_transaction is null)
            {
                var transactionId = Guid.NewGuid();
                var effectiveBusinessOperation = ResolveBusinessOperation(businessOperation);
                _transaction = _session.BeginTransaction();
                _sqlCaptureScope = SqlStatementCapture.Begin(transactionId, effectiveBusinessOperation);

                _logger?.LogDebug(
                    "NHibernate transaction started. BusinessOperation={BusinessOperation} TransactionId={TransactionId}",
                    effectiveBusinessOperation,
                    transactionId);
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error beginning NHibernate transaction.");
            DisposeSqlCaptureScope();
            throw new DataException("Error beginning NHibernate transaction.", ex);
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();

                var sqlStatements = GetCapturedSqlStatements();
                var businessOperations = GetCapturedBusinessOperations();
                _logger?.LogInformation(
                    "NHibernate transaction committed successfully. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatementCount={SqlStatementCount}",
                    _sqlCaptureScope?.BusinessOperation,
                    businessOperations,
                    _sqlCaptureScope?.TransactionId,
                    sqlStatements.Count);

                if (sqlStatements.Count > 0)
                {
                    _logger?.LogDebug(
                        "NHibernate SQL statements committed. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatements={SqlStatements}",
                        _sqlCaptureScope?.BusinessOperation,
                        businessOperations,
                        _sqlCaptureScope?.TransactionId,
                        sqlStatements);
                }

                _transaction.Dispose();
                _transaction = null;
                DisposeSqlCaptureScope();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                ex,
                "Error committing NHibernate transaction. TransactionId={TransactionId}",
                _sqlCaptureScope?.TransactionId);
            throw new DataException("Error committing NHibernate transaction.", ex);
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();

                var sqlStatements = GetCapturedSqlStatements();
                var businessOperations = GetCapturedBusinessOperations();
                _logger?.LogInformation(
                    "NHibernate transaction rolled back. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatementCount={SqlStatementCount}",
                    _sqlCaptureScope?.BusinessOperation,
                    businessOperations,
                    _sqlCaptureScope?.TransactionId,
                    sqlStatements.Count);

                if (sqlStatements.Count > 0)
                {
                    _logger?.LogDebug(
                        "NHibernate SQL statements rolled back. BusinessOperation={BusinessOperation} BusinessOperations={BusinessOperations} TransactionId={TransactionId} SqlStatements={SqlStatements}",
                        _sqlCaptureScope?.BusinessOperation,
                        businessOperations,
                        _sqlCaptureScope?.TransactionId,
                        sqlStatements);
                }

                _transaction.Dispose();
                _transaction = null;
                DisposeSqlCaptureScope();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                ex,
                "Error rolling back NHibernate transaction. TransactionId={TransactionId}",
                _sqlCaptureScope?.TransactionId);
            throw new DataException("Error rolling back NHibernate transaction.", ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_transaction is not null)
        {
            await RollbackAsync();
        }

        _session.Dispose();
        _disposed = true;
    }

    private IReadOnlyList<string> GetCapturedSqlStatements() =>
        _sqlCaptureScope?.GetStatements() ?? Array.Empty<string>();

    private IReadOnlyList<string> GetCapturedBusinessOperations() =>
        _sqlCaptureScope?.GetBusinessOperations() ?? Array.Empty<string>();

    private static string ResolveBusinessOperation(string? businessOperation)
    {
        if (!string.IsNullOrWhiteSpace(businessOperation))
        {
            return businessOperation;
        }

        return "UnitOfWork transaction";
    }

    private void DisposeSqlCaptureScope()
    {
        _sqlCaptureScope?.Dispose();
        _sqlCaptureScope = null;
    }
}
