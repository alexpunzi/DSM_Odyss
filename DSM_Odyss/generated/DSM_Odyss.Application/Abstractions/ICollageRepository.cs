using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Abstractions;

public interface ICollageRepository : IRepository<Collage>
{
    Task<IReadOnlyList<Collage>> DelAnyoAsync(int p_anyo);
}
