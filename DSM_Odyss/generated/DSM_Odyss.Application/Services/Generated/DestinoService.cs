using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class DestinoService : ServiceEntity<Destino>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDestinoRepository _repository;

    public DestinoService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public DestinoService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private DestinoService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.DestinoRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.DestinoRepository;
    }

    public Task<Destino> AddDestinoDefaultAsync(Destino entity) =>
        AddDefaultAsync(entity);

    public Task DeleteDestinoDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Destino?> ReadOIDDestinoDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Destino>> ReadAllDestinoDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Destino> ModifierDestinoDefaultAsync(Destino entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Destino> CrearDestinoAsync(string p_pais)
    {
        Destino entity = new Destino
        {
            Pais = p_pais,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarDestinoAsync(int p_oid)
    {
        Destino entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Destino not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarDestinoAsync(int p_oid, string p_pais)
    {
        Destino entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Destino not found.");
        entity.Pais = p_pais;
        await _repository.SaveAsync(entity);
    }

    public async Task<Destino?> LeerDestinoOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Destino>> LeerTodasDestinosAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
