using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class EstadisticasService : ServiceEntity<Estadisticas>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEstadisticasRepository _repository;

    public EstadisticasService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public EstadisticasService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private EstadisticasService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.EstadisticasRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.EstadisticasRepository;
    }

    public Task<Estadisticas> AddEstadisticasDefaultAsync(Estadisticas entity) =>
        AddDefaultAsync(entity);

    public Task DeleteEstadisticasDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Estadisticas?> ReadOIDEstadisticasDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Estadisticas>> ReadAllEstadisticasDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Estadisticas> ModifierEstadisticasDefaultAsync(Estadisticas entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Estadisticas> CrearEstadisticasAsync(int p_cliente_oid)
    {
        Estadisticas entity = new Estadisticas
        {
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarEstadisticasAsync(int p_oid)
    {
        Estadisticas entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Estadisticas not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarEstadisticasAsync(int p_oid, int p_cliente_oid)
    {
        Estadisticas entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Estadisticas not found.");
        await _repository.SaveAsync(entity);
    }

    public async Task<Estadisticas?> LeerEstadisticasOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Estadisticas>> LeerTodasEstadisticasAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
