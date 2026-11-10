// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(MultimediaController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(MultimediaController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class MultimediaController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(MultimediaController_SubirContenidoMultimedia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> SubirContenidoMultimedia(int p_carpeta_oid, TipoMultimedia p_tipo, string p_nombre)
    {
    /*PROTECTED REGION ID(MultimediaController_SubirContenidoMultimedia_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _multimediaService.SubirContenidoMultimediaAsync(p_carpeta_oid, p_tipo, p_nombre);
        return Ok(result);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(MultimediaController_MoverDeCarpeta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> MoverDeCarpeta(int p_oid, int p_nuevaCarpeta_oid)
    {
    /*PROTECTED REGION ID(MultimediaController_MoverDeCarpeta_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _multimediaService.MoverDeCarpetaAsync(p_oid, p_nuevaCarpeta_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
