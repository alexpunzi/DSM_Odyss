// Código manual (no generado por HARIA): pantalla de "Cambiar contraseña" (operación custom cambiarPassword).
using Microsoft.AspNetCore.Mvc;

namespace DSM_Odyss.Web.Controllers;

public partial class ClienteController
{
    // GET /Cliente/CambiarPassword → formulario. El guardado lo hace la acción POST CambiarPassword
    // de ClienteController.Manual.cs.
    [HttpGet]
    public IActionResult CambiarPassword()
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();
        ViewData["ClienteId"] = clienteId;
        return View();
    }
}
