// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class CollageAssembler
{
    /*PROTECTED REGION ID(CollageAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.CollageViewModel ToViewModel(Collage entity) =>
        new()
        {
            Id = entity.Id,
            UrlImagen = entity.UrlImagen,
            FechaCreacion = entity.FechaCreacion,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CollageAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.CollageViewModel model, Collage entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.UrlImagen = model.UrlImagen;
        entity.FechaCreacion = model.FechaCreacion;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(CollageAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
