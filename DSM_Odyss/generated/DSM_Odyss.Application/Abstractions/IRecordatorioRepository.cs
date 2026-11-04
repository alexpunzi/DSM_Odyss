using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IRecordatorioRepository : IRepository<Recordatorio>
{
    Task<IReadOnlyList<Recordatorio>> PendientesAsync();
}
