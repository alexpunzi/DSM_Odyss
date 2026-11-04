using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_compartirItinerario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task CompartirItinerarioAsync(int p_oid, int p_destinatario_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_compartirItinerario) ENABLED START*/
        Viaje viaje = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        Cliente destinatario = await _unitOfWork.ClienteRepository.ReadAsync(p_destinatario_oid)
            ?? throw new ModelException("Recipient client not found.");

        if (!viaje.CompartidoCon.Any(cliente => cliente.Id == destinatario.Id))
        {
            viaje.CompartidoCon.Add(destinatario);
            await _repository.SaveAsync(viaje);
        }
        /*PROTECTED REGION END*/
    }
}
