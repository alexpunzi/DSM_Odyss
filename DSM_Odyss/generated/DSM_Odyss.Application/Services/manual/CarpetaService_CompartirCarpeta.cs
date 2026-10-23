using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Carpeta_compartirCarpeta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class CarpetaService
{
    public async Task CompartirCarpetaAsync(int p_oid, int p_cliente_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Carpeta_compartirCarpeta) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Carpeta.compartirCarpeta is pending.", new NotImplementedException("Method CompartirCarpetaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
