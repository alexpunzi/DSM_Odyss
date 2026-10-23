using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Cliente_agregarContacto) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService
{
    public async Task AgregarContactoAsync(int p_oid, int p_contacto_oid, string p_alias)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Cliente_agregarContacto) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Cliente.agregarContacto is pending.", new NotImplementedException("Method AgregarContactoAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
