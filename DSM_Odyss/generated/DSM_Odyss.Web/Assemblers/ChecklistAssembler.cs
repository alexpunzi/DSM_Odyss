// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ChecklistAssembler
{
    /*PROTECTED REGION ID(ChecklistAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ChecklistViewModel ToViewModel(Checklist entity) =>
        new()
        {
            Id = entity.Id,
            Nombre = entity.Nombre,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ChecklistAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ChecklistViewModel model, Checklist entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Nombre = model.Nombre;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ChecklistAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
