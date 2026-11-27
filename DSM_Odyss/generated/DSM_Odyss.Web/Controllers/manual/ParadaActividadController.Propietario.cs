// Código manual (no generado por HARIA): solo el autor del viaje puede cambiar sus actividades.
namespace DSM_Odyss.Web.Controllers;

public partial class ParadaActividadController
{
    // Datos del viaje que necesitamos fuera de la sesión de base de datos.
    private sealed record ViajeDeActividad(int Id, DateTime FechaInicio, DateTime FechaFin);

    // Devuelve el viaje de la actividad si pertenece al usuario conectado; si no, null.
    private async Task<ViajeDeActividad?> ViajeDeActividadPropiaAsync(int actividadId)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return null;

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var actividad = await unitOfWork.ParadaActividadRepository.ReadAsync(actividadId);
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        if (actividad?.Viaje is null || cliente is null) return null;

        var viaje = cliente.Viajes.FirstOrDefault(v => v.Id == actividad.Viaje.Id);
        // Copiamos los datos aquí, mientras la sesión sigue abierta.
        return viaje is null ? null : new ViajeDeActividad(viaje.Id, viaje.FechaInicio, viaje.FechaFin);
    }
}
