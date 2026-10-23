using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;
using DSM_Odyss.Domain.ValueObjects;
using DSM_Odyss.Infrastructure.Exceptions;

namespace DSM_Odyss.Infrastructure.Repositories;

public sealed class UbicacionRepository : RepositoryBase<Ubicacion>, IUbicacionRepository
{
    public UbicacionRepository(ILogger<UbicacionRepository>? logger = null)
        : base(logger)
    {
    }

    public UbicacionRepository(ISession session, ILogger<UbicacionRepository>? logger = null)
        : base(session, logger)
    {
    }

    public UbicacionRepository(ISessionFactory sessionFactory, ILogger<UbicacionRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Ubicacion>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Ubicacion>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Ubicacion?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Ubicacion>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Ubicacion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Ubicacion entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }


}