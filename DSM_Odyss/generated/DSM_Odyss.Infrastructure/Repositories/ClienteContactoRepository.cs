using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Infrastructure.Exceptions;

namespace DSM_Odyss.Infrastructure.Repositories;

public sealed class ClienteContactoRepository : RepositoryBase<ClienteContacto>, IClienteContactoRepository
{
    public ClienteContactoRepository(ILogger<ClienteContactoRepository>? logger = null)
        : base(logger)
    {
    }

    public ClienteContactoRepository(ISession session, ILogger<ClienteContactoRepository>? logger = null)
        : base(session, logger)
    {
    }

    public ClienteContactoRepository(ISessionFactory sessionFactory, ILogger<ClienteContactoRepository>? logger = null)
        : base(sessionFactory, logger)
    {
    }

    public async Task<IReadOnlyList<ClienteContacto>> ReadAllAsync()
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.Query<ClienteContacto>().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading all {Entity} entities.", EntityName);
            throw new DataException($"Error reading all {EntityName} entities.", ex);
        }
    }

    public async Task<ClienteContacto?> ReadAsync(object id)
    {
        try
        {
            using var context = OpenSession();
            return await context.Session.GetAsync<ClienteContacto>(id);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Error reading {Entity} entity with id {EntityId}.", EntityName, id);
            throw new DataException($"Error reading {EntityName} entity.", ex);
        }
    }

    public Task SaveAsync(ClienteContacto entity) =>
        ExecuteRepositoryOperationInTransactionAsync(
            "Save",
            session => session.SaveOrUpdateAsync(entity),
            $"Error saving {EntityName} entity.");

    public Task DeleteAsync(ClienteContacto entity) =>
        ExecuteRepositoryOperationInTransactionAsync(
            "Delete",
            session => session.DeleteAsync(entity),
            $"Error deleting {EntityName} entity.");
}
