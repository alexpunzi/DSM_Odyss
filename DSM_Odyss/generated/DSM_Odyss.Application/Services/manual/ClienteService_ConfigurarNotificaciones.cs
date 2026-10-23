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
        // Write here your custom code...

        throw new ModelException("Application service generation for Cliente.configurarNotificaciones is pending.", new NotImplementedException("Method ConfigurarNotificacionesAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
