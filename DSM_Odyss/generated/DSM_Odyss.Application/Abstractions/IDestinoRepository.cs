using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IDestinoRepository : IRepository<Destino>
{
    Task<IReadOnlyList<Destino>> PorPaisAsync(string p_pais);
}
