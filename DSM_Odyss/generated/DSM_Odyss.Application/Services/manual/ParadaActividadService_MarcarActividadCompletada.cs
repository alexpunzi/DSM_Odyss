using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_ParadaActividad_marcarActividadCompletada) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaActividadService
{
    public async Task MarcarActividadCompletadaAsync(int p_oid, bool p_completada)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_ParadaActividad_marcarActividadCompletada) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for ParadaActividad.marcarActividadCompletada is pending.", new NotImplementedException("Method MarcarActividadCompletadaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
