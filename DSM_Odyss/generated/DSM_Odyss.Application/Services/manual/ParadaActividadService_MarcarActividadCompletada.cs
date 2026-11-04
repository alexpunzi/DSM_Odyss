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
        ParadaActividad actividad = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Activity not found.");
        actividad.Completada = p_completada;
        await _repository.SaveAsync(actividad);
        /*PROTECTED REGION END*/
    }
}
