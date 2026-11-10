// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ResenaAssembler
{
    /*PROTECTED REGION ID(ResenaAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ResenaViewModel ToViewModel(Resena entity) =>
        new()
        {
            Id = entity.Id,
            Comentario = entity.Comentario,
            Fecha = entity.Fecha,
            Puntuacion = entity.Puntuacion,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ResenaAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ResenaViewModel model, Resena entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Comentario = model.Comentario;
        entity.Fecha = model.Fecha;
        entity.Puntuacion = model.Puntuacion;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ResenaAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
