// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class TareaAssembler
{
    /*PROTECTED REGION ID(TareaAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.TareaViewModel ToViewModel(Tarea entity) =>
        new()
        {
            Id = entity.Id,
            Descripcion = entity.Descripcion,
            Completada = entity.Completada,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(TareaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.TareaViewModel model, Tarea entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Descripcion = model.Descripcion;
        entity.Completada = model.Completada;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(TareaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
