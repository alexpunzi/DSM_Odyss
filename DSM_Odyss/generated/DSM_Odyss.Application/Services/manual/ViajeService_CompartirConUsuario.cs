using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_compartirConUsuario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task CompartirConUsuarioAsync(int p_oid, int p_cliente_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_compartirConUsuario) ENABLED START*/
        await CompartirItinerarioAsync(p_oid, p_cliente_oid);
        /*PROTECTED REGION END*/
    }
}
