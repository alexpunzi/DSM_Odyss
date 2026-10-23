using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class RecordatorioService : ServiceEntity<Recordatorio>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecordatorioRepository _repository;

    public RecordatorioService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public RecordatorioService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private RecordatorioService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.RecordatorioRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.RecordatorioRepository;
    }

    public Task<Recordatorio> AddRecordatorioDefaultAsync(Recordatorio entity) =>
        AddDefaultAsync(entity);

    public Task DeleteRecordatorioDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Recordatorio?> ReadOIDRecordatorioDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Recordatorio>> ReadAllRecordatorioDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Recordatorio> ModifierRecordatorioDefaultAsync(Recordatorio entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Recordatorio> CrearRecordatorioAsync(string p_titulo, string p_detalles, DateTime p_fecha, DateTime p_hora, TipoAviso p_tipoAviso, bool p_enviado)
    {
        Recordatorio entity = new Recordatorio
        {
            Titulo = p_titulo,
            Detalles = p_detalles,
            Fecha = p_fecha,
            Hora = p_hora,
            TipoAviso = p_tipoAviso,
            Enviado = p_enviado,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarRecordatorioAsync(int p_oid)
    {
        Recordatorio entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Recordatorio not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarRecordatorioAsync(int p_oid, string p_titulo, string p_detalles, DateTime p_fecha, DateTime p_hora, TipoAviso p_tipoAviso, bool p_enviado)
    {
        Recordatorio entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Recordatorio not found.");
        entity.Titulo = p_titulo;
        entity.Detalles = p_detalles;
        entity.Fecha = p_fecha;
        entity.Hora = p_hora;
        entity.TipoAviso = p_tipoAviso;
        entity.Enviado = p_enviado;
        await _repository.SaveAsync(entity);
    }

    public async Task<Recordatorio?> LeerRecordatorioOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Recordatorio>> LeerTodosRecordatoriosAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
