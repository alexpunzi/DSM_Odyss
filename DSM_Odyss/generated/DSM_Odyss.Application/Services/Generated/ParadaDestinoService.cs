using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaDestinoService : ServiceEntity<ParadaDestino>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParadaDestinoRepository _repository;

    public ParadaDestinoService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ParadaDestinoService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ParadaDestinoService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ParadaDestinoRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ParadaDestinoRepository;
    }

    public Task<ParadaDestino> AddParadaDestinoDefaultAsync(ParadaDestino entity) =>
        AddDefaultAsync(entity);

    public Task DeleteParadaDestinoDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<ParadaDestino?> ReadOIDParadaDestinoDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<ParadaDestino>> ReadAllParadaDestinoDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<ParadaDestino> ModifierParadaDestinoDefaultAsync(ParadaDestino entity) =>
        ModifierDefaultAsync(entity);

    public async Task<ParadaDestino> CrearParadaDestinoAsync(int p_orden, DateTime p_fechaLlegada, DateTime p_fechaSalida, int p_viaje_oid, int p_destino_oid)
    {
        Viaje viajeEntity = await _unitOfWork.ViajeRepository.ReadAsync(p_viaje_oid)
            ?? throw new ModelException("Viaje not found for ParadaDestino.Viaje.");

        Destino destinoEntity = await _unitOfWork.DestinoRepository.ReadAsync(p_destino_oid)
            ?? throw new ModelException("Destino not found for ParadaDestino.Destino.");

        ParadaDestino entity = new ParadaDestino
        {
            Orden = p_orden,
            FechaLlegada = p_fechaLlegada,
            FechaSalida = p_fechaSalida,
            Viaje = viajeEntity,
            Destino = destinoEntity,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarParadaDestinoAsync(int p_oid)
    {
        ParadaDestino entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaDestino not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarParadaDestinoAsync(int p_oid, int p_orden, DateTime p_fechaLlegada, DateTime p_fechaSalida, int p_viaje_oid, int p_destino_oid)
    {
        ParadaDestino entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaDestino not found.");
        entity.Orden = p_orden;
        entity.FechaLlegada = p_fechaLlegada;
        entity.FechaSalida = p_fechaSalida;
        await _repository.SaveAsync(entity);
    }

    public async Task<ParadaDestino?> LeerParadaDestinoOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<ParadaDestino>> LeerTodasParadasDestinoAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<ParadaDestino>> PorViajeAsync(int p_viaje_oid)
    {
        return await _repository.PorViajeAsync(p_viaje_oid);
    }

    public async Task AsignarActividadesAsync(int p_oid, IReadOnlyList<int> p_actividades_oid)
    {
        ParadaDestino entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("ParadaDestino not found.");
        entity.Actividades ??= new List<ParadaActividad>();
        foreach (var relatedId in p_actividades_oid)
        {
            ParadaActividad relatedEntity = await _unitOfWork.ParadaActividadRepository.ReadAsync(relatedId)
                ?? throw new ModelException("ParadaActividad not found for ParadaDestino.Actividades.");
            if (!entity.Actividades.Contains(relatedEntity))
            {
                entity.Actividades.Add(relatedEntity);
            }
            relatedEntity.ParadaDestino = entity;
            await _unitOfWork.ParadaActividadRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

}
