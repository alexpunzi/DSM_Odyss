// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ViajeAssembler
{
    /*PROTECTED REGION ID(ViajeAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ViajeViewModel ToViewModel(Viaje entity) =>
        new()
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            FechaInicio = entity.FechaInicio,
            FechaFin = entity.FechaFin,
            EsPublico = entity.EsPublico,
            Presupuesto = entity.Presupuesto,
            TipoViaje = entity.TipoViaje,
            Estado = entity.Estado,
            CarpetasId = entity.Carpetas?.Id,
            CarpetasLabel = entity.Carpetas is null ? null : Convert.ToString(entity.Carpetas.Nombre, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            CollagesId = entity.Collages?.Id,
            CollagesLabel = entity.Collages is null ? null : Convert.ToString(entity.Collages.UrlImagen, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ViajeViewModel model, Viaje entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Titulo = model.Titulo;
        entity.FechaInicio = model.FechaInicio;
        entity.FechaFin = model.FechaFin;
        entity.EsPublico = model.EsPublico;
        entity.Presupuesto = model.Presupuesto;
        entity.TipoViaje = model.TipoViaje;
        entity.Estado = model.Estado;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
