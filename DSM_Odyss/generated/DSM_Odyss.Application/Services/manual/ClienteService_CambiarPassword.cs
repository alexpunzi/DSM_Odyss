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
        Cliente cliente = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Cliente not found.");

        if (!_passwordHasher.VerifyPassword(cliente.Password, p_oldPass))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(p_newPass)
            || p_newPass.Length < 8
            || !p_newPass.Any(char.IsDigit)
            || !p_newPass.Any(character => !char.IsLetterOrDigit(character)))
        {
            throw new ModelException("The new password must have at least 8 characters, one number and one special character.");
        }

        if (_passwordHasher.VerifyPassword(cliente.Password, p_newPass))
        {
            return false;
        }

        cliente.Password = _passwordHasher.HashPasswordIfNeeded(p_newPass);
        await _repository.SaveAsync(cliente);
        return true;
        /*PROTECTED REGION END*/
    }
}
