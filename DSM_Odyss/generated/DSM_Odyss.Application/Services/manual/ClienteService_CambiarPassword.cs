using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Cliente_cambiarPassword) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService
{
    public async Task<bool> CambiarPasswordAsync(int p_oid, string p_oldPass, string p_newPass)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Cliente_cambiarPassword) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Cliente.cambiarPassword is pending.", new NotImplementedException("Method CambiarPasswordAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
