using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<IReadOnlyList<Cliente>> ConRecordatoriosActivosAsync();
    Task<IReadOnlyList<Cliente>> ConViajesPublicosAsync();
}
