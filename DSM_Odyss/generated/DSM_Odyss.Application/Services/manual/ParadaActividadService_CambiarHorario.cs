using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_ParadaActividad_cambiarHorario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaActividadService
{
    public async Task CambiarHorarioAsync(int p_oid, int p_nuevoDia, DateTime p_nuevoHorario)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_ParadaActividad_cambiarHorario) ENABLED START*/
        if (p_nuevoDia < 0)
        {
            throw new ModelException("The itinerary day cannot be negative.");
        }

        ParadaActividad actividad = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Activity not found.");
        actividad.Dia = p_nuevoDia;
        actividad.Horario = p_nuevoHorario;
        await _repository.SaveAsync(actividad);
        /*PROTECTED REGION END*/
    }
}
