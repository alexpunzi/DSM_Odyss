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
        await CambiarHorarioAsync(p_oid, p_nuevoDia, p_nuevoHorario);
        /*PROTECTED REGION END*/
    }
}
