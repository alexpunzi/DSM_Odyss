// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class UbicacionAssembler
{
    /*PROTECTED REGION ID(UbicacionAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.UbicacionViewModel ToViewModel(Ubicacion entity) =>
        new()
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Latitud = entity.Latitud,
            Longitud = entity.Longitud,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(UbicacionAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.UbicacionViewModel model, Ubicacion entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Nombre = model.Nombre;
        entity.Latitud = model.Latitud;
        entity.Longitud = model.Longitud;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(UbicacionAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
