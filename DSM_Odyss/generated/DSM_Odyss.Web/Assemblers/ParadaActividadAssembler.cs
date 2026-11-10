// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class ParadaActividadAssembler
{
    /*PROTECTED REGION ID(ParadaActividadAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.ParadaActividadViewModel ToViewModel(ParadaActividad entity) =>
        new()
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Dia = entity.Dia,
            Horario = entity.Horario,
            Detalles = entity.Detalles,
            Completada = entity.Completada,
            ViajeId = entity.Viaje?.Id,
            ViajeLabel = entity.Viaje is null ? null : Convert.ToString(entity.Viaje.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            UbicacionId = entity.Ubicacion?.Id,
            UbicacionLabel = entity.Ubicacion is null ? null : Convert.ToString(entity.Ubicacion.Nombre, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            ParadaDestinoId = entity.ParadaDestino?.Id,
            ParadaDestinoLabel = entity.ParadaDestino is null ? null : Convert.ToString(entity.ParadaDestino.Id, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaActividadAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.ParadaActividadViewModel model, ParadaActividad entity)
    {
        // Use validated input and a server-loaded entity. Persist through Application.
        // Identifiers, secrets and stored file paths are never copied from the client.
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Nombre = model.Nombre;
        entity.Dia = model.Dia;
        entity.Horario = model.Horario;
        entity.Detalles = model.Detalles;
        entity.Completada = model.Completada;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaActividadAssembler_custom) ENABLED START*/
    // Add custom properties, validation helpers or actions here.
    /*PROTECTED REGION END*/
}
