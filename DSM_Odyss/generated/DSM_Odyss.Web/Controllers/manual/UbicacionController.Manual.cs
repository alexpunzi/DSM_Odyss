// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(UbicacionController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(UbicacionController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class UbicacionController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UbicacionController_ConsultarUbicacionEnMapa_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConsultarUbicacionEnMapa(int p_oid)
    {
    /*PROTECTED REGION ID(UbicacionController_ConsultarUbicacionEnMapa_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _ubicacionService.ConsultarUbicacionEnMapaAsync(p_oid);
        return Ok(result);
    /*PROTECTED REGION END*/
    }
}
