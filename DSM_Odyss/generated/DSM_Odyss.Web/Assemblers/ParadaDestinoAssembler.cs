// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ParadaDestinoAssembler
{
    /*PROTECTED REGION ID(ParadaDestinoAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel ToViewModel(ParadaDestino entity) =>
        new()
        {
            Id = entity.Id,
            Orden = entity.Orden,
            FechaLlegada = entity.FechaLlegada,
            FechaSalida = entity.FechaSalida,
            ViajeId = entity.Viaje?.Id,
            ViajeLabel = entity.Viaje is null ? null : Convert.ToString(entity.Viaje.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            DestinoId = entity.Destino?.Id,
            DestinoLabel = entity.Destino is null ? null : Convert.ToString(entity.Destino.Pais, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaDestinoAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel model, ParadaDestino entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Orden = model.Orden;
        entity.FechaLlegada = model.FechaLlegada;
        entity.FechaSalida = model.FechaSalida;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaDestinoAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
