using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;
using DSM_Odyss.Domain.ValueObjects;
using DSM_Odyss.Infrastructure.Exceptions;

namespace DSM_Odyss.Infrastructure.Repositories;

public sealed class TareaRepository : RepositoryBase<Tarea>, ITareaRepository
{
    public TareaRepository(ILogger<TareaRepository>? logger = null)
        : base(logger)
    {
    }

    public TareaRepository(ISession session, ILogger<TareaRepository>? logger = null)
        : base(session, logger)
    {
    }

    public TareaRepository(ISessionFactory sessionFactory, ILogger<TareaRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<Tarea>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<Tarea>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<Tarea?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<Tarea>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public async Task SaveAsync(Tarea entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");
    }

    public async Task DeleteAsync(Tarea entity)
    {
        await ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
    }


}