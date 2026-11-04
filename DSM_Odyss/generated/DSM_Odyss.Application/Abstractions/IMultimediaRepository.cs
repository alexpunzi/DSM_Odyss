using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Abstractions;

public interface IMultimediaRepository : IRepository<Multimedia>
{
    Task<IReadOnlyList<Multimedia>> DelTipoAsync(TipoMultimedia p_tipo);
}
