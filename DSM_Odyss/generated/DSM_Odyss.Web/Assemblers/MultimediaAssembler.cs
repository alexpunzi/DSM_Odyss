// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class MultimediaAssembler
{
    /*PROTECTED REGION ID(MultimediaAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.MultimediaViewModel ToViewModel(Multimedia entity) =>
        new()
        {
            Id = entity.Id,
            Tipo = entity.Tipo,
            Nombre = entity.Nombre,
            FechaSubida = entity.FechaSubida,
            Descripcion = entity.Descripcion,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(MultimediaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.MultimediaViewModel model, Multimedia entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Tipo = model.Tipo;
        entity.Nombre = model.Nombre;
        entity.FechaSubida = model.FechaSubida;
        entity.Descripcion = model.Descripcion;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(MultimediaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
