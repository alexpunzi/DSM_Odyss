using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface IResenaRepository : IRepository<Resena>
{
    Task<IReadOnlyList<Resena>> ConPuntuacionMinimaAsync(int p_puntuacion);
}
