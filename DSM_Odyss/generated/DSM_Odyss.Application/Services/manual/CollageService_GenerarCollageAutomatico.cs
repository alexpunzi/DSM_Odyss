using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Collage_generarCollageAutomatico) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class CollageService
{
    public async Task<Collage> GenerarCollageAutomaticoAsync(int p_viaje_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Collage_generarCollageAutomatico) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Collage.generarCollageAutomatico is pending.", new NotImplementedException("Method GenerarCollageAutomaticoAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
