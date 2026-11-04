using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_duplicarViaje) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task<Viaje> DuplicarViajeAsync(int p_oid, string p_nuevoTitulo)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_duplicarViaje) ENABLED START*/
        return await DuplicarItinerarioAsync(p_oid, p_nuevoTitulo);
        /*PROTECTED REGION END*/
    }
}
