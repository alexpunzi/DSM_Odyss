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
        // Write here your custom code...

        throw new ModelException("Application service generation for Tarea.cambiarEstadoTarea is pending.", new NotImplementedException("Method CambiarEstadoTareaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
