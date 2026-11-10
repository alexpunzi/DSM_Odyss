// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class DestinoAssembler
{
    /*PROTECTED REGION ID(DestinoAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.DestinoViewModel ToViewModel(Destino entity) =>
        new()
        {
            Id = entity.Id,
            Pais = entity.Pais,
            Ciudad = entity.Ciudad,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DestinoAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.DestinoViewModel model, Destino entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Pais = model.Pais;
        entity.Ciudad = model.Ciudad;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(DestinoAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
