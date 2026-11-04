using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IUbicacionRepository : IRepository<Ubicacion>
{
    Task<IReadOnlyList<Ubicacion>> ConNombreAsync(string p_nombre);
}
