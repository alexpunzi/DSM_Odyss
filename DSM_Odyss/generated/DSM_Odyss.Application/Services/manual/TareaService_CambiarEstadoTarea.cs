using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Tarea_cambiarEstadoTarea) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class TareaService
{
    public async Task CambiarEstadoTareaAsync(int p_oid, bool p_completada)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Tarea_cambiarEstadoTarea) ENABLED START*/
        Tarea tarea = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Tarea not found.");
        tarea.Completada = p_completada;
        await _repository.SaveAsync(tarea);
        /*PROTECTED REGION END*/
    }
}
