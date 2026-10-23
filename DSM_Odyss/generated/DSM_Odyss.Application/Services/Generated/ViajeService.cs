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

    public async Task<Viaje> CrearViajeAsync(string p_titulo, DateTime p_fechaInicio, DateTime p_fechaFin, bool p_esPublico, float p_presupuesto, TipoViaje p_tipoViaje, EstadoViaje p_estado, int p_destino_oid, int p_autor_oid, int p_carpetas_oid)
    {
        Destino destinoEntity = await _unitOfWork.DestinoRepository.ReadAsync(p_destino_oid)
            ?? throw new ModelException("Destino not found for Viaje.Destino.");

        Viaje entity = new Viaje
        {
            Titulo = p_titulo,
            FechaInicio = p_fechaInicio,
            FechaFin = p_fechaFin,
            EsPublico = p_esPublico,
            Presupuesto = p_presupuesto,
            TipoViaje = p_tipoViaje,
            Estado = p_estado,
            Destino = destinoEntity,
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

    public async Task ModificarViajeAsync(int p_oid, string p_titulo, DateTime p_fechaInicio, DateTime p_fechaFin, bool p_esPublico, float p_presupuesto, TipoViaje p_tipoViaje, EstadoViaje p_estado, int p_destino_oid, int p_autor_oid, int p_carpetas_oid)
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

}
