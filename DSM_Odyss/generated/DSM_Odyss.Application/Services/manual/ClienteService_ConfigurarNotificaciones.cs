using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Cliente_configurarNotificaciones) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService
{
    public async Task ConfigurarNotificacionesAsync(int p_oid, CanalNotificacion p_canal, bool p_recibirRecordatorios)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Cliente_configurarNotificaciones) ENABLED START*/
        if (!Enum.IsDefined(p_canal))
        {
            throw new ModelException("The notification channel is not supported.");
        }

        Cliente cliente = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Cliente not found.");
        cliente.CanalNotificacion = p_canal;
        cliente.RecibirRecordatorios = p_recibirRecordatorios;
        await _repository.SaveAsync(cliente);
        /*PROTECTED REGION END*/
    }
}
