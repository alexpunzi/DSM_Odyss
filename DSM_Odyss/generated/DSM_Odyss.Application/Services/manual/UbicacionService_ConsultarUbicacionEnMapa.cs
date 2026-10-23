using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Ubicacion_consultarUbicacionEnMapa) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class UbicacionService
{
    public async Task<string> ConsultarUbicacionEnMapaAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Ubicacion_consultarUbicacionEnMapa) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Ubicacion.consultarUbicacionEnMapa is pending.", new NotImplementedException("Method ConsultarUbicacionEnMapaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
