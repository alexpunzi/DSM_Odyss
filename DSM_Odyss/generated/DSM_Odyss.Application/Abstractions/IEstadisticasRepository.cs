using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IEstadisticasRepository : IRepository<Estadisticas>
{
    Task<IReadOnlyList<Estadisticas>> DelAnyoAsync(int p_anyo);
}
