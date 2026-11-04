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
        Viaje viaje = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        viaje.EsPublico = p_esPublico;
        await _repository.SaveAsync(viaje);
        /*PROTECTED REGION END*/
    }
}
