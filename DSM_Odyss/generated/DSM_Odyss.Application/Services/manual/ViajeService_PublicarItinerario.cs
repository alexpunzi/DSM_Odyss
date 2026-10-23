using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_publicarItinerario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task PublicarItinerarioAsync(int p_oid, bool p_esPublico)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_publicarItinerario) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Viaje.publicarItinerario is pending.", new NotImplementedException("Method PublicarItinerarioAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
