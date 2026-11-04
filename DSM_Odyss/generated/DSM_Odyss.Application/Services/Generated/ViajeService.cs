using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService : ServiceEntity<Viaje>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IViajeRepository _repository;

    public ViajeService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ViajeService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ViajeService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ViajeRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ViajeRepository;
    }

    public Task<Viaje> AddViajeDefaultAsync(Viaje entity) =>
        AddDefaultAsync(entity);

    public Task DeleteViajeDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Viaje?> ReadOIDViajeDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Viaje>> ReadAllViajeDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Viaje> ModifierViajeDefaultAsync(Viaje entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Viaje> CrearViajeAsync(string p_titulo, DateTime p_fechaInicio, DateTime p_fechaFin, bool p_esPublico, float p_presupuesto, TipoViaje p_tipoViaje, EstadoViaje p_estado, int p_autor_oid, int p_carpetas_oid)
    {
        Viaje entity = new Viaje
        {
            Titulo = p_titulo,
            FechaInicio = p_fechaInicio,
            FechaFin = p_fechaFin,
            EsPublico = p_esPublico,
            Presupuesto = p_presupuesto,
            TipoViaje = p_tipoViaje,
            Estado = p_estado,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarViajeAsync(int p_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarViajeAsync(int p_oid, string p_titulo, DateTime p_fechaInicio, DateTime p_fechaFin, bool p_esPublico, float p_presupuesto, TipoViaje p_tipoViaje, EstadoViaje p_estado, int p_autor_oid, int p_carpetas_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.Titulo = p_titulo;
        entity.FechaInicio = p_fechaInicio;
        entity.FechaFin = p_fechaFin;
        entity.EsPublico = p_esPublico;
        entity.Presupuesto = p_presupuesto;
        entity.TipoViaje = p_tipoViaje;
        entity.Estado = p_estado;
        await _repository.SaveAsync(entity);
    }

    public async Task<Viaje?> LeerViajeOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Viaje>> LeerTodosViajesAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<Viaje>> ViajesPublicosAsync()
    {
        return await _repository.ViajesPublicosAsync();
    }

    public async Task<IReadOnlyList<Viaje>> ViajesPorEstadoAsync(EstadoViaje p_estado)
    {
        return await _repository.ViajesPorEstadoAsync(p_estado);
    }

    public async Task AgregarParadaAsync(int p_oid, IReadOnlyList<int> p_paradas_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.Paradas ??= new List<ParadaActividad>();
        foreach (var relatedId in p_paradas_oid)
        {
            ParadaActividad relatedEntity = await _unitOfWork.ParadaActividadRepository.ReadAsync(relatedId)
                ?? throw new ModelException("ParadaActividad not found for Viaje.Paradas.");
            if (!entity.Paradas.Contains(relatedEntity))
            {
                entity.Paradas.Add(relatedEntity);
            }
            relatedEntity.Viaje = entity;
            await _unitOfWork.ParadaActividadRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

    public async Task AgregarParadaDestinoAsync(int p_oid, IReadOnlyList<int> p_paradasDestino_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.ParadasDestino ??= new List<ParadaDestino>();
        foreach (var relatedId in p_paradasDestino_oid)
        {
            ParadaDestino relatedEntity = await _unitOfWork.ParadaDestinoRepository.ReadAsync(relatedId)
                ?? throw new ModelException("ParadaDestino not found for Viaje.ParadasDestino.");
            if (!entity.ParadasDestino.Contains(relatedEntity))
            {
                entity.ParadasDestino.Add(relatedEntity);
            }
            relatedEntity.Viaje = entity;
            await _unitOfWork.ParadaDestinoRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

    public async Task CompartirConClienteAsync(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.CompartidoCon ??= new List<Cliente>();
        foreach (var relatedId in p_clientes_oid)
        {
            Cliente relatedEntity = await _unitOfWork.ClienteRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Cliente not found for Viaje.CompartidoCon.");
            if (!entity.CompartidoCon.Contains(relatedEntity))
            {
                entity.CompartidoCon.Add(relatedEntity);
            }
        }
        await _repository.SaveAsync(entity);
    }

    public async Task DejarDeCompartirConClienteAsync(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.CompartidoCon ??= new List<Cliente>();
        foreach (var relatedId in p_clientes_oid)
        {
            Cliente relatedEntity = await _unitOfWork.ClienteRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Cliente not found for Viaje.CompartidoCon.");
            if (!entity.CompartidoCon.Remove(relatedEntity))
            {
                throw new ModelException("Cliente is not related to Viaje.CompartidoCon.");
            }
        }
        await _repository.SaveAsync(entity);
    }

    public async Task AgregarResenaAsync(int p_oid, IReadOnlyList<int> p_resenas_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        entity.Resenas ??= new List<Resena>();
        foreach (var relatedId in p_resenas_oid)
        {
            Resena relatedEntity = await _unitOfWork.ResenaRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Resena not found for Viaje.Resenas.");
            if (!entity.Resenas.Contains(relatedEntity))
            {
                entity.Resenas.Add(relatedEntity);
            }
        }
        await _repository.SaveAsync(entity);
    }

    public async Task AsignarCarpetaAsync(int p_oid, int p_carpeta_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        Carpeta relatedEntity = await _unitOfWork.CarpetaRepository.ReadAsync(p_carpeta_oid)
            ?? throw new ModelException("Carpeta not found for Viaje.Carpetas.");
        entity.Carpetas = relatedEntity;
        await _repository.SaveAsync(entity);
    }

    public async Task AsignarCollageAsync(int p_oid, int p_collage_oid)
    {
        Viaje entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        Collage relatedEntity = await _unitOfWork.CollageRepository.ReadAsync(p_collage_oid)
            ?? throw new ModelException("Collage not found for Viaje.Collages.");
        entity.Collages = relatedEntity;
        await _repository.SaveAsync(entity);
    }

}
