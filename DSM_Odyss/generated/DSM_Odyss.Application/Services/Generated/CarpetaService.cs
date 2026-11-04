using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class CarpetaService : ServiceEntity<Carpeta>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICarpetaRepository _repository;

    public CarpetaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public CarpetaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private CarpetaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.CarpetaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.CarpetaRepository;
    }

    public Task<Carpeta> AddCarpetaDefaultAsync(Carpeta entity) =>
        AddDefaultAsync(entity);

    public Task DeleteCarpetaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Carpeta?> ReadOIDCarpetaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Carpeta>> ReadAllCarpetaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Carpeta> ModifierCarpetaDefaultAsync(Carpeta entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Carpeta> CrearCarpetaAsync(string p_nombre, int p_viaje_oid)
    {
        Carpeta entity = new Carpeta
        {
            Nombre = p_nombre,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarCarpetaAsync(int p_oid)
    {
        Carpeta entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarCarpetaAsync(int p_oid, string p_nombre, int p_viaje_oid)
    {
        Carpeta entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        entity.Nombre = p_nombre;
        await _repository.SaveAsync(entity);
    }

    public async Task<Carpeta?> LeerCarpetaOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Carpeta>> LeerTodasCarpetasAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IReadOnlyList<Carpeta>> ConMultimediaAsync()
    {
        return await _repository.ConMultimediaAsync();
    }

    public async Task AgregarMultimediaAsync(int p_oid, IReadOnlyList<int> p_multimedia_oid)
    {
        Carpeta entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        entity.Multimedia ??= new List<Multimedia>();
        foreach (var relatedId in p_multimedia_oid)
        {
            Multimedia relatedEntity = await _unitOfWork.MultimediaRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Multimedia not found for Carpeta.Multimedia.");
            if (!entity.Multimedia.Contains(relatedEntity))
            {
                entity.Multimedia.Add(relatedEntity);
            }
        }
        await _repository.SaveAsync(entity);
    }

    public async Task CompartirConClienteAsync(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
        Carpeta entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        entity.CompartidoCon ??= new List<Cliente>();
        foreach (var relatedId in p_clientes_oid)
        {
            Cliente relatedEntity = await _unitOfWork.ClienteRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Cliente not found for Carpeta.CompartidoCon.");
            if (!entity.CompartidoCon.Contains(relatedEntity))
            {
                entity.CompartidoCon.Add(relatedEntity);
            }
            relatedEntity.CarpetasCompartidas ??= new List<Carpeta>();
            if (!relatedEntity.CarpetasCompartidas.Contains(entity))
            {
                relatedEntity.CarpetasCompartidas.Add(entity);
            }
            await _unitOfWork.ClienteRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

    public async Task DejarDeCompartirConClienteAsync(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
        Carpeta entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        entity.CompartidoCon ??= new List<Cliente>();
        foreach (var relatedId in p_clientes_oid)
        {
            Cliente relatedEntity = await _unitOfWork.ClienteRepository.ReadAsync(relatedId)
                ?? throw new ModelException("Cliente not found for Carpeta.CompartidoCon.");
            if (!entity.CompartidoCon.Remove(relatedEntity))
            {
                throw new ModelException("Cliente is not related to Carpeta.CompartidoCon.");
            }
            relatedEntity.CarpetasCompartidas ??= new List<Carpeta>();
            relatedEntity.CarpetasCompartidas.Remove(entity);
            await _unitOfWork.ClienteRepository.SaveAsync(relatedEntity);
        }
        await _repository.SaveAsync(entity);
    }

}
