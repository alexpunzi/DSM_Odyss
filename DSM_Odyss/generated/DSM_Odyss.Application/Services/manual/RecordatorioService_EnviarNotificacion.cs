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

        // Cada recordatorio pertenece a un cliente (ClienteRecordatorio): lo buscamos entre los clientes.
        IReadOnlyList<Cliente> clientes = await _unitOfWork.ClienteRepository.ReadAllAsync();
        Cliente? destinatario = clientes.FirstOrDefault(cliente =>
            cliente.Recordatorios.Any(item => item.Id == recordatorio.Id));
        if (destinatario is null)
        {
            throw new ModelException("The reminder has no recipients.");
        }

        if (!destinatario.RecibirRecordatorios)
        {
            throw new ModelException("No recipient has reminders enabled.");
        }

        // The current model stores delivery status but has no email/SMS provider to call.
        recordatorio.Enviado = true;
        await _repository.SaveAsync(recordatorio);
        /*PROTECTED REGION END*/
    }
}
