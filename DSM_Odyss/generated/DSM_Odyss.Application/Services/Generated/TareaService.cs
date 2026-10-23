using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class TareaService : ServiceEntity<Tarea>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITareaRepository _repository;

    public TareaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public TareaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private TareaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.TareaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.TareaRepository;
    }

    public Task<Tarea> AddTareaDefaultAsync(Tarea entity) =>
        AddDefaultAsync(entity);

    public Task DeleteTareaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Tarea?> ReadOIDTareaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Tarea>> ReadAllTareaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Tarea> ModifierTareaDefaultAsync(Tarea entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Tarea> CrearTareaAsync(string p_descripcion, bool p_completada, int p_checklist_oid)
    {
        Tarea entity = new Tarea
        {
            Descripcion = p_descripcion,
            Completada = p_completada,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarTareaAsync(int p_oid)
    {
        Tarea entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Tarea not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarTareaAsync(int p_oid, string p_descripcion, bool p_completada, int p_checklist_oid)
    {
        Tarea entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Tarea not found.");
        entity.Descripcion = p_descripcion;
        entity.Completada = p_completada;
        await _repository.SaveAsync(entity);
    }

    public async Task<Tarea?> LeerTareaOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Tarea>> LeerTodasTareasAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
