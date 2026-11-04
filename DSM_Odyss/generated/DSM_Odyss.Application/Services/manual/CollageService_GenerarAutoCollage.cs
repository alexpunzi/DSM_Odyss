using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Collage_generarAutoCollage) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class CollageService
{
    public async Task<Collage> GenerarAutoCollageAsync(int p_viaje_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Collage_generarAutoCollage) ENABLED START*/
        return await GenerarCollageAutomaticoAsync(p_viaje_oid);
        /*PROTECTED REGION END*/
    }
}
