// Código manual (no generado por HARIA): cada recordatorio pertenece a un usuario.
namespace DSM_Odyss.Web.Controllers;

public partial class RecordatorioController
{
    // Solo su destinatario puede ver, editar o borrar un recordatorio.
    private async Task<bool> EsMiRecordatorioAsync(int recordatorioId)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return false;
        var cliente = await _clienteService.LeerClienteOIDAsync(clienteId);
        return cliente?.Recordatorios.Any(r => r.Id == recordatorioId) == true;
    }
}