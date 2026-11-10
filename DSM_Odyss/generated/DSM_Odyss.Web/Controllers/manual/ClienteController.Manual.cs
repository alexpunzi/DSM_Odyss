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

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _clienteService.CambiarPasswordAsync(p_oid, p_oldPass, p_newPass);
        return Ok(result);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_ConfigurarNotificaciones_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConfigurarNotificaciones(int p_oid, CanalNotificacion p_canal, bool p_recibirRecordatorios)
    {
    /*PROTECTED REGION ID(ClienteController_ConfigurarNotificaciones_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.ConfigurarNotificacionesAsync(p_oid, p_canal, p_recibirRecordatorios);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
