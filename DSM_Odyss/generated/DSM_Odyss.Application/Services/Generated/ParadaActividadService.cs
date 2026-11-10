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
        Viaje viajeEntity = await _unitOfWork.ViajeRepository.ReadAsync(p_viaje_oid)
            ?? throw new ModelException("Viaje not found for ParadaActividad.Viaje.");

        ParadaActividad entity = new ParadaActividad
        {
            Nombre = p_nombre,
            Dia = p_dia,
            Horario = p_horario,
            Completada = p_completada,
            Viaje = viajeEntity,
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

    public async Task<IReadOnlyList<ParadaActividad>> PendientesAsync()
    {
        return await _repository.PendientesAsync();
    }

    public async Task AsignarUbicacionAsync(int p_oid, int p_ubicacion_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        Ubicacion relatedEntity = await _unitOfWork.UbicacionRepository.ReadAsync(p_ubicacion_oid)
            ?? throw new ModelException("Ubicacion not found for ParadaActividad.Ubicacion.");
        entity.Ubicacion = relatedEntity;
        await _repository.SaveAsync(entity);
    }

    public async Task DesasignarUbicacionAsync(int p_oid, int p_ubicacion_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        Ubicacion relatedEntity = await _unitOfWork.UbicacionRepository.ReadAsync(p_ubicacion_oid)
            ?? throw new ModelException("Ubicacion not found for ParadaActividad.Ubicacion.");
        if (entity.Ubicacion is null || !entity.Ubicacion.Equals(relatedEntity))
        {
            throw new ModelException("Ubicacion is not related to ParadaActividad.Ubicacion.");
        }
        entity.Ubicacion = null!;
        await _repository.SaveAsync(entity);
    }

    public async Task AsignarParadaDestinoAsync(int p_oid, int p_paradaDestino_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        ParadaDestino relatedEntity = await _unitOfWork.ParadaDestinoRepository.ReadAsync(p_paradaDestino_oid)
            ?? throw new ModelException("ParadaDestino not found for ParadaActividad.ParadaDestino.");
        entity.ParadaDestino = relatedEntity;
        relatedEntity.Actividades ??= new List<ParadaActividad>();
        if (!relatedEntity.Actividades.Contains(entity))
        {
            relatedEntity.Actividades.Add(entity);
        }
        await _unitOfWork.ParadaDestinoRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

    public async Task DesasignarParadaDestinoAsync(int p_oid, int p_paradaDestino_oid)
    {
        ParadaActividad entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaActividad not found.");
        ParadaDestino relatedEntity = await _unitOfWork.ParadaDestinoRepository.ReadAsync(p_paradaDestino_oid)
            ?? throw new ModelException("ParadaDestino not found for ParadaActividad.ParadaDestino.");
        if (entity.ParadaDestino is null || !entity.ParadaDestino.Equals(relatedEntity))
        {
            throw new ModelException("ParadaDestino is not related to ParadaActividad.ParadaDestino.");
        }
        relatedEntity.Actividades ??= new List<ParadaActividad>();
        relatedEntity.Actividades.Remove(entity);
        entity.ParadaDestino = null!;
        await _unitOfWork.ParadaDestinoRepository.SaveAsync(relatedEntity);
        await _repository.SaveAsync(entity);
    }

}
