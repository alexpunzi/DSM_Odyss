// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ClienteController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;
using DSM_Odyss.Web.Utils.Security;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(ClienteController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ClienteController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_AgregarContacto_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarContacto(int p_oid, int p_contacto_oid, string p_alias)
    {
    /*PROTECTED REGION ID(ClienteController_AgregarContacto_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.AgregarContactoAsync(p_oid, p_contacto_oid, p_alias);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_CambiarPassword_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CambiarPassword(int p_oid, string p_oldPass, string p_newPass)
    {
    /*PROTECTED REGION ID(ClienteController_CambiarPassword_POST_body) ENABLED START*/

        // Vista: formulario de Views/Cliente/CambiarPassword.cshtml (solo tu propia contraseña).
        if (!int.TryParse(User.Identity?.Name, out var clienteId) || clienteId != p_oid) return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["PasswordError"] = "Rellena todos los campos.";
            return RedirectToAction(nameof(CambiarPassword));
        }
        if (p_newPass != Request.Form["p_confirmPass"].ToString())
        {
            TempData["PasswordError"] = "Las dos contraseñas nuevas no coinciden.";
            return RedirectToAction(nameof(CambiarPassword));
        }

        try
        {
            var cambiada = await _clienteService.CambiarPasswordAsync(p_oid, p_oldPass, p_newPass);
            if (!cambiada)
            {
                TempData["PasswordError"] = "La contraseña actual no es correcta, o la nueva es igual que la anterior.";
                return RedirectToAction(nameof(CambiarPassword));
            }
        }
        catch (DSM_Odyss.Application.Exceptions.ModelException)
        {
            TempData["PasswordError"] = "La nueva contraseña debe tener al menos 8 caracteres, un número y un carácter especial.";
            return RedirectToAction(nameof(CambiarPassword));
        }

        TempData["PasswordOk"] = "Contraseña cambiada correctamente.";
        return RedirectToAction(nameof(CambiarPassword));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_ConfigurarNotificaciones_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConfigurarNotificaciones(int p_oid, CanalNotificacion p_canal, bool p_recibirRecordatorios)
    {
    /*PROTECTED REGION ID(ClienteController_ConfigurarNotificaciones_POST_body) ENABLED START*/

        // Solo puedes cambiar tus propias preferencias
        if (!int.TryParse(User.Identity?.Name, out var clienteId) || clienteId != p_oid) return Forbid();

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.ConfigurarNotificacionesAsync(p_oid, p_canal, p_recibirRecordatorios);

        TempData["MensajeNotificaciones"] = "Preferencias de notificación guardadas.";
        return RedirectToAction("Index", "Recordatorio");
    /*PROTECTED REGION END*/
    }
}
