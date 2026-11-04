using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Recordatorio_enviarNotificacion) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class RecordatorioService
{
    public async Task EnviarNotificacionAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Recordatorio_enviarNotificacion) ENABLED START*/
        Recordatorio recordatorio = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Recordatorio not found.");

        if (recordatorio.Enviado)
        {
            return;
        }

        DateTime fechaEnvio = recordatorio.Fecha.Date + recordatorio.Hora.TimeOfDay;
        if (fechaEnvio > DateTime.Now)
        {
            throw new ModelException("The reminder cannot be sent before its scheduled date and time.");
        }

        if (recordatorio.Recordatorios.Count == 0)
        {
            throw new ModelException("The reminder has no recipients.");
        }

        if (!recordatorio.Recordatorios.Any(cliente => cliente.RecibirRecordatorios))
        {
            throw new ModelException("No recipient has reminders enabled.");
        }

        // The current model stores delivery status but has no email/SMS provider to call.
        recordatorio.Enviado = true;
        await _repository.SaveAsync(recordatorio);
        /*PROTECTED REGION END*/
    }
}
