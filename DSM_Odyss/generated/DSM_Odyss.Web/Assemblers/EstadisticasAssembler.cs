// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class EstadisticasAssembler
{
    /*PROTECTED REGION ID(EstadisticasAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.EstadisticasViewModel ToViewModel(Estadisticas entity) =>
        new()
        {
            Id = entity.Id,
            Anyo = entity.Anyo,
            PaisesVisitados = entity.PaisesVisitados,
            FotosSubidas = entity.FotosSubidas,
            DestinoMasVisitado = entity.DestinoMasVisitado,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(EstadisticasAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.EstadisticasViewModel model, Estadisticas entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Anyo = model.Anyo;
        entity.PaisesVisitados = model.PaisesVisitados;
        entity.FotosSubidas = model.FotosSubidas;
        entity.DestinoMasVisitado = model.DestinoMasVisitado;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(EstadisticasAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
