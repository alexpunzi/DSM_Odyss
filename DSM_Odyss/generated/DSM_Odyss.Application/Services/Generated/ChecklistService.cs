using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ChecklistService : ServiceEntity<Checklist>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChecklistRepository _repository;

    public ChecklistService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ChecklistService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ChecklistService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ChecklistRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ChecklistRepository;
    }

    public Task<Checklist> AddChecklistDefaultAsync(Checklist entity) =>
        AddDefaultAsync(entity);

    public Task DeleteChecklistDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Checklist?> ReadOIDChecklistDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Checklist>> ReadAllChecklistDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Checklist> ModifierChecklistDefaultAsync(Checklist entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Checklist> CrearChecklistAsync(string p_nombre)
    {
        Checklist entity = new Checklist
        {
            Nombre = p_nombre,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarChecklistAsync(int p_oid)
    {
        Checklist entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Checklist not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarChecklistAsync(int p_oid, string p_nombre)
    {
        Checklist entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Checklist not found.");
        entity.Nombre = p_nombre;
        await _repository.SaveAsync(entity);
    }

    public async Task<Checklist?> LeerChecklistOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Checklist>> LeerTodosChecklistsAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
