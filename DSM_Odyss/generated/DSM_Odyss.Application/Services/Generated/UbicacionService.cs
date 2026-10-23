using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class UbicacionService : ServiceEntity<Ubicacion>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUbicacionRepository _repository;

    public UbicacionService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public UbicacionService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private UbicacionService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.UbicacionRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.UbicacionRepository;
    }

    public Task<Ubicacion> AddUbicacionDefaultAsync(Ubicacion entity) =>
        AddDefaultAsync(entity);

    public Task DeleteUbicacionDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Ubicacion?> ReadOIDUbicacionDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Ubicacion>> ReadAllUbicacionDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Ubicacion> ModifierUbicacionDefaultAsync(Ubicacion entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Ubicacion> CrearUbicacionAsync(string p_nombre, double p_latitud, double p_longitud)
    {
        Ubicacion entity = new Ubicacion
        {
            Nombre = p_nombre,
            Latitud = p_latitud,
            Longitud = p_longitud,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarUbicacionAsync(int p_oid)
    {
        Ubicacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Ubicacion not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarUbicacionAsync(int p_oid, string p_nombre, double p_latitud, double p_longitud)
    {
        Ubicacion entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Ubicacion not found.");
        entity.Nombre = p_nombre;
        entity.Latitud = p_latitud;
        entity.Longitud = p_longitud;
        await _repository.SaveAsync(entity);
    }

    public async Task<Ubicacion?> LeerUbicacionOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Ubicacion>> LeerTodasUbicacionesAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
