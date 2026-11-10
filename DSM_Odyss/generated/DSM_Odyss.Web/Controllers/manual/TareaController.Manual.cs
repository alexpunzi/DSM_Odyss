// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(TareaController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(TareaController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class TareaController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TareaController_CambiarEstadoTarea_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CambiarEstadoTarea(int p_oid, bool p_completada)
    {
    /*PROTECTED REGION ID(TareaController_CambiarEstadoTarea_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _tareaService.CambiarEstadoTareaAsync(p_oid, p_completada);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
