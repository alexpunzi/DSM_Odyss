using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface ITareaRepository : IRepository<Tarea>
{
    Task<IReadOnlyList<Tarea>> PorEstadoAsync(bool p_completada);
}
