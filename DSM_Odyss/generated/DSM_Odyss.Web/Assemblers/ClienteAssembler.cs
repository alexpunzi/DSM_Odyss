// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ClienteAssembler
{
    /*PROTECTED REGION ID(ClienteAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ClienteViewModel ToViewModel(Cliente entity) =>
        new()
        {
            Id = entity.Id,
            User = entity.User,
            Nombre = entity.Nombre,
            Apellidos = entity.Apellidos,
            Email = entity.Email,
            FechaNacimiento = entity.FechaNacimiento,
            CanalNotificacion = entity.CanalNotificacion,
            RecibirRecordatorios = entity.RecibirRecordatorios,

        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ClienteAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ClienteViewModel model, Cliente entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.User = model.User;
        entity.Nombre = model.Nombre;
        entity.Apellidos = model.Apellidos;
        entity.Email = model.Email;
        entity.FechaNacimiento = model.FechaNacimiento;
        entity.CanalNotificacion = model.CanalNotificacion;
        entity.RecibirRecordatorios = model.RecibirRecordatorios;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ClienteAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
