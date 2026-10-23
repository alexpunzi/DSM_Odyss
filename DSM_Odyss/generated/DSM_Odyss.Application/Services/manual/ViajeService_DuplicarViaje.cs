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
        // Write here your custom code...

        throw new ModelException("Application service generation for Viaje.duplicarViaje is pending.", new NotImplementedException("Method DuplicarViajeAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
