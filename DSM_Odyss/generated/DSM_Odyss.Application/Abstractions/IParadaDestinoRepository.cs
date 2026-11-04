using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IParadaDestinoRepository : IRepository<ParadaDestino>
{
    Task<IReadOnlyList<ParadaDestino>> PorViajeAsync(int p_viaje_oid);
}
