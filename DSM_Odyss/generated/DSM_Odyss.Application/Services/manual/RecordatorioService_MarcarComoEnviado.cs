using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Recordatorio_marcarComoEnviado) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class RecordatorioService
{
    public async Task MarcarComoEnviadoAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Recordatorio_marcarComoEnviado) ENABLED START*/
        await EnviarNotificacionAsync(p_oid);
        /*PROTECTED REGION END*/
    }
}
