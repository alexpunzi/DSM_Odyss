using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Cliente_actualizarPreferenciaNotificacion) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService
{
    public async Task ActualizarPreferenciaNotificacionAsync(int p_oid, CanalNotificacion p_canal, bool p_recibir)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Cliente_actualizarPreferenciaNotificacion) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Cliente.actualizarPreferenciaNotificacion is pending.", new NotImplementedException("Method ActualizarPreferenciaNotificacionAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
