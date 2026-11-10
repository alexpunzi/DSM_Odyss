// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class CarpetaAssembler
{
    /*PROTECTED REGION ID(CarpetaAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.CarpetaViewModel ToViewModel(Carpeta entity) =>
        new()
        {
            Id = entity.Id,
            Nombre = entity.Nombre,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CarpetaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.CarpetaViewModel model, Carpeta entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Nombre = model.Nombre;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CarpetaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
