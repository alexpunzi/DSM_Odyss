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
        Carpeta carpeta = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Carpeta not found.");
        Cliente cliente = await _unitOfWork.ClienteRepository.ReadAsync(p_cliente_oid)
            ?? throw new ModelException("Cliente not found.");

        // The join table keeps album permissions separate from the client's profile.
        if (!carpeta.CompartidoCon.Any(item => item.Id == cliente.Id))
        {
            carpeta.CompartidoCon.Add(cliente);
            await _repository.SaveAsync(carpeta);
        }
        /*PROTECTED REGION END*/
    }
}
