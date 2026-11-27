// Código manual (no generado por HARIA): pantalla de "Configurar notificaciones" (CU-14).
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.Controllers;

public partial class ClienteController
{
    // GET /Cliente/ConfigurarNotificaciones → formulario con las preferencias actuales.
    // El guardado lo hace la acción POST ConfigurarNotificaciones de ClienteController.Manual.cs.
    [HttpGet]
    public async Task<IActionResult> ConfigurarNotificaciones()
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();

        var cliente = await _clienteService.LeerClienteOIDAsync(clienteId);
        if (cliente is null) return NotFound();

        var model = new ConfigurarNotificacionesViewModel
        {
            ClienteId = cliente.Id,
            Canal = cliente.CanalNotificacion,
            RecibirRecordatorios = cliente.RecibirRecordatorios
        };
        return View(model);
    }
}