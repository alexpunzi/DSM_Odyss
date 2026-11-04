using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_cambiarEstadoViaje) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task CambiarEstadoViajeAsync(int p_oid, EstadoViaje p_nuevoEstado)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_cambiarEstadoViaje) ENABLED START*/
        if (!Enum.IsDefined(p_nuevoEstado))
        {
            throw new ModelException("The trip state is not supported.");
        }

        Viaje viaje = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        viaje.Estado = p_nuevoEstado;
        await _repository.SaveAsync(viaje);
        /*PROTECTED REGION END*/
    }
}
