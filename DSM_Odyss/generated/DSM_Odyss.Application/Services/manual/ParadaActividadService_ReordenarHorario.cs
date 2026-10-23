using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_ParadaActividad_reordenarHorario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaActividadService
{
    public async Task ReordenarHorarioAsync(int p_oid, int p_nuevoDia, DateTime p_nuevoHorario)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_ParadaActividad_reordenarHorario) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for ParadaActividad.reordenarHorario is pending.", new NotImplementedException("Method ReordenarHorarioAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
