using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaActividadService : ServiceEntity<ParadaActividad>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParadaActividadRepository _repository;

    public ParadaActividadService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ParadaActividadService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ParadaActividadService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ParadaActividadRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ParadaActividadRepository;
    }

    public Task<ParadaActividad> AddParadaActividadDefaultAsync(ParadaActividad entity) =>
        AddDefaultAsync(entity);

    public Task DeleteParadaActividadDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<ParadaActividad?> ReadOIDParadaActividadDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<ParadaActividad>> ReadAllParadaActividadDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<ParadaActividad> ModifierParadaActividadDefaultAsync(ParadaActividad entity) =>
        ModifierDefaultAsync(entity);

    public async Task<ParadaActividad> CrearParadaActividadAsync(string p_nombre, int p_dia, DateTime p_horario, bool p_completada, int p_viaje_oid)
    {
        ParadaActividad entity = new ParadaActividad
        {
            Nombre = p_nombre,
            Dia = p_dia,
            Horario = p_horario,
            Completada = p_completada,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarParadaActividadAsync(int p_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarParadaActividadAsync(int p_oid, string p_nombre, int p_dia, DateTime p_horario, bool p_completada, int p_viaje_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        entity.Nombre = p_nombre;
        entity.Dia = p_dia;
        entity.Horario = p_horario;
        entity.Completada = p_completada;
        await _repository.SaveAsync(entity);
    }

    public async Task<ParadaActividad?> LeerParadaActividadOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<ParadaActividad>> LeerTodasParadasActividadesAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
