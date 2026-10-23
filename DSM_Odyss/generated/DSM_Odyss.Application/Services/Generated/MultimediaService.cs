using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class MultimediaService : ServiceEntity<Multimedia>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMultimediaRepository _repository;

    public MultimediaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public MultimediaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private MultimediaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.MultimediaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.MultimediaRepository;
    }

    public Task<Multimedia> AddMultimediaDefaultAsync(Multimedia entity) =>
        AddDefaultAsync(entity);

    public Task DeleteMultimediaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Multimedia?> ReadOIDMultimediaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Multimedia>> ReadAllMultimediaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Multimedia> ModifierMultimediaDefaultAsync(Multimedia entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Multimedia> CrearMultimediaAsync(TipoMultimedia p_tipo, string p_nombre, DateTime p_fechaSubida, string p_descripcion, int p_carpeta_oid)
    {
        Multimedia entity = new Multimedia
        {
            Tipo = p_tipo,
            Nombre = p_nombre,
            FechaSubida = p_fechaSubida,
            Descripcion = p_descripcion,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarMultimediaAsync(int p_oid)
    {
        Multimedia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Multimedia not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarMultimediaAsync(int p_oid, TipoMultimedia p_tipo, string p_nombre, DateTime p_fechaSubida, string p_descripcion, int p_carpeta_oid)
    {
        Multimedia entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Multimedia not found.");
        entity.Tipo = p_tipo;
        entity.Nombre = p_nombre;
        entity.FechaSubida = p_fechaSubida;
        entity.Descripcion = p_descripcion;
        await _repository.SaveAsync(entity);
    }

    public async Task<Multimedia?> LeerMultimediaOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Multimedia>> LeerTodasMultimdiaAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
