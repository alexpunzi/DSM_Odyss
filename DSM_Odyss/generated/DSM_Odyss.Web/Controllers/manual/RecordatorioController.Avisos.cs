// Código manual (no generado por HARIA): CU-15 Recibir notificación.
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Controllers;

public partial class RecordatorioController
{
    // Envía (operación custom EnviarNotificacion) los recordatorios del cliente
    // cuya fecha y hora ya han llegado y que todavía no se habían enviado.
    // Devuelve los títulos de los recordatorios enviados.
    private async Task<List<string>> EnviarAvisosVencidosAsync(Cliente cliente)
    {
        var enviados = new List<string>();

        // Si el usuario ha desactivado los recordatorios en la configuración, no se envía nada.
        if (!cliente.RecibirRecordatorios) return enviados;

        var ahora = DateTime.Now;
        var vencidos = cliente.Recordatorios
            .Where(r => !r.Enviado && r.Fecha.Date + r.Hora.TimeOfDay <= ahora)
            .ToList();

        foreach (var recordatorio in vencidos)
        {
            try
            {
                await _recordatorioService.EnviarNotificacionAsync(recordatorio.Id);
                enviados.Add(recordatorio.Titulo);
            }
            catch (ModelException error)
            {
                // Si uno falla, seguimos con los demás
                _logger.LogWarning(error, "No se pudo enviar el recordatorio {RecordatorioId}.", recordatorio.Id);
            }
        }

        return enviados;
    }
}