using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ResenaService : ServiceEntity<Resena>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResenaRepository _repository;

    public ResenaService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ResenaService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ResenaService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ResenaRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ResenaRepository;
    }

    public Task<Resena> AddResenaDefaultAsync(Resena entity) =>
        AddDefaultAsync(entity);

    public Task DeleteResenaDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Resena?> ReadOIDResenaDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Resena>> ReadAllResenaDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Resena> ModifierResenaDefaultAsync(Resena entity) =>
        ModifierDefaultAsync(entity);

    public async Task<Resena> CrearResenaAsync(string p_comentario, DateTime p_fecha, int p_puntuacion, int p_viaje_oid, int p_cliente_oid)
    {
        Resena entity = new Resena
        {
            Comentario = p_comentario,
            Fecha = p_fecha,
            Puntuacion = p_puntuacion,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarResenaAsync(int p_oid)
    {
        Resena entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Resena not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarResenaAsync(int p_oid, string p_comentario, DateTime p_fecha, int p_puntuacion, int p_viaje_oid, int p_cliente_oid)
    {
        Resena entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Resena not found.");
        entity.Comentario = p_comentario;
        entity.Fecha = p_fecha;
        entity.Puntuacion = p_puntuacion;
        await _repository.SaveAsync(entity);
    }

    public async Task<Resena?> LeerResenaOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Resena>> LeerTodasResenasAsync()
    {
        return await _repository.ReadAllAsync();
    }

}
