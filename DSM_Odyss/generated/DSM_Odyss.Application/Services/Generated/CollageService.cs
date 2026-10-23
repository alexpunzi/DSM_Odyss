using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class CollageService : ServiceEntity<Collage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICollageRepository _repository;

    public CollageService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public CollageService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private CollageService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.CollageRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.CollageRepository;
    }

    public Task<Collage> AddCollageDefaultAsync(Collage entity) =>
        AddDefaultAsync(entity);

    public Task DeleteCollageDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Collage?> ReadOIDCollageDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Collage>> ReadAllCollageDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Collage> ModifierCollageDefaultAsync(Collage entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Collage> CrearCollageAsync(string p_urlImagen, DateTime p_fechaCreacion, int p_viaje_oid)
    {
        Collage entity = new Collage
        {
            UrlImagen = p_urlImagen,
            FechaCreacion = p_fechaCreacion,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarCollageAsync(int p_oid)
    {
        Collage entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Collage not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarCollageAsync(int p_oid, string p_urlImagen, DateTime p_fechaCreacion, int p_viaje_oid)
    {
        Collage entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Collage not found.");
        entity.UrlImagen = p_urlImagen;
        entity.FechaCreacion = p_fechaCreacion;
        await _repository.SaveAsync(entity);
    }

    public async Task<Collage?> LeerCollageOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Collage>> LeerTodasCollagesAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
