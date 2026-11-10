// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ViajeController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION ID(ViajeController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ViajeController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_CompartirItinerario_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CompartirItinerario(int p_oid, int p_destinatario_oid)
    {
    /*PROTECTED REGION ID(ViajeController_CompartirItinerario_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.CompartirItinerarioAsync(p_oid, p_destinatario_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_PublicarItinerario_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PublicarItinerario(int p_oid, bool p_esPublico)
    {
    /*PROTECTED REGION ID(ViajeController_PublicarItinerario_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.PublicarItinerarioAsync(p_oid, p_esPublico);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_DuplicarItinerario_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DuplicarItinerario(int p_oid, string p_nuevoTitulo)
    {
    /*PROTECTED REGION ID(ViajeController_DuplicarItinerario_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _viajeService.DuplicarItinerarioAsync(p_oid, p_nuevoTitulo);
        return Ok(result);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_CambiarEstadoViaje_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CambiarEstadoViaje(int p_oid, EstadoViaje p_nuevoEstado)
    {
    /*PROTECTED REGION ID(ViajeController_CambiarEstadoViaje_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.CambiarEstadoViajeAsync(p_oid, p_nuevoEstado);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
