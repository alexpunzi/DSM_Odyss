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
        if (p_oid == p_contacto_oid)
        {
            throw new ModelException("A client cannot add themselves as a contact.");
        }

        string alias = p_alias?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ModelException("A contact alias is required.");
        }

        Cliente propietario = await _unitOfWork.ClienteRepository.ReadAsync(p_oid)
            ?? throw new ModelException("Cliente not found.");
        Cliente contacto = await _unitOfWork.ClienteRepository.ReadAsync(p_contacto_oid)
            ?? throw new ModelException("Contact client not found.");

        // The alias belongs to this client-contact pair, not to the contact's global profile.
        IReadOnlyList<ClienteContacto> contactos = await _unitOfWork.ClienteContactoRepository.ReadAllAsync();
        ClienteContacto? relacion = contactos.FirstOrDefault(item =>
            item.Propietario.Id == propietario.Id && item.Contacto.Id == contacto.Id);

        if (relacion is null)
        {
            relacion = new ClienteContacto
            {
                Propietario = propietario,
                Contacto = contacto,
                Alias = alias,
            };
        }
        else
        {
            relacion.Alias = alias;
        }

        await _unitOfWork.ClienteContactoRepository.SaveAsync(relacion);
        /*PROTECTED REGION END*/
    }
}
