using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Abstractions;

public interface IViajeRepository : IRepository<Viaje>
{
    Task<IReadOnlyList<Viaje>> ViajesPublicosAsync();
    Task<IReadOnlyList<Viaje>> ViajesPorEstadoAsync(EstadoViaje p_estado);
}
