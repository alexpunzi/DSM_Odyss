// Código manual (no generado por HARIA): cada checklist pertenece a un usuario.
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Controllers;

public partial class ChecklistController
{
    // Cliente que ha iniciado sesión (HARIA guarda su Id en User.Identity.Name).
    private async Task<Cliente?> ClienteConectadoAsync() =>
        int.TryParse(User.Identity?.Name, out var id)
            ? await _clienteService.LeerClienteOIDAsync(id)
            : null;

    // Solo el propietario puede ver, editar o borrar un checklist.
    private async Task<bool> EsMiChecklistAsync(int checklistId)
    {
        var cliente = await ClienteConectadoAsync();
        return cliente?.Checklists.Any(c => c.Id == checklistId) == true;
    }
}