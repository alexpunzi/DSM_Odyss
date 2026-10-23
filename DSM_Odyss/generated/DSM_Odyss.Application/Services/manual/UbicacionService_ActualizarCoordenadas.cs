using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Ubicacion_actualizarCoordenadas) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class UbicacionService
{
    public async Task ActualizarCoordenadasAsync(int p_oid, double p_latitud, double p_longitud)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Ubicacion_actualizarCoordenadas) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Ubicacion.actualizarCoordenadas is pending.", new NotImplementedException("Method ActualizarCoordenadasAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
