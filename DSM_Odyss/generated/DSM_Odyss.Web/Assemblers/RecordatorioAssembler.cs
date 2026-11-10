// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class RecordatorioAssembler
{
    /*PROTECTED REGION ID(RecordatorioAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.RecordatorioViewModel ToViewModel(Recordatorio entity) =>
        new()
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Detalles = entity.Detalles,
            Fecha = entity.Fecha,
            Hora = entity.Hora,
            TipoAviso = entity.TipoAviso,
            Enviado = entity.Enviado,
            TareaId = entity.Tarea?.Id,
            TareaLabel = entity.Tarea is null ? null : Convert.ToString(entity.Tarea.Descripcion, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RecordatorioAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.RecordatorioViewModel model, Recordatorio entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Titulo = model.Titulo;
        entity.Detalles = model.Detalles;
        entity.Fecha = model.Fecha;
        entity.Hora = model.Hora;
        entity.TipoAviso = model.TipoAviso;
        entity.Enviado = model.Enviado;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RecordatorioAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
