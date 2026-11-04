namespace DSM_Odyss.Application.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    IClienteRepository ClienteRepository { get; }
    IEstadisticasRepository EstadisticasRepository { get; }
    IViajeRepository ViajeRepository { get; }
    IParadaActividadRepository ParadaActividadRepository { get; }
    IUbicacionRepository UbicacionRepository { get; }
    IDestinoRepository DestinoRepository { get; }
    ICarpetaRepository CarpetaRepository { get; }
    IMultimediaRepository MultimediaRepository { get; }
    ICollageRepository CollageRepository { get; }
    IRecordatorioRepository RecordatorioRepository { get; }
    IChecklistRepository ChecklistRepository { get; }
    ITareaRepository TareaRepository { get; }
    IResenaRepository ResenaRepository { get; }

    IHariaPasswordHasher PasswordHasher { get; }
    IHariaTokenGenerator TokenGenerator { get; }

    Task BeginTransactionAsync(string? businessOperation = null);
    Task CommitAsync();
    Task RollbackAsync();
}
