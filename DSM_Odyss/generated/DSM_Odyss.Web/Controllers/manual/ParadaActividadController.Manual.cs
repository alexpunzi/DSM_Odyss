// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ParadaActividadController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(ParadaActividadController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ParadaActividadController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_MarcarActividadCompletada_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> MarcarActividadCompletada(int p_oid, bool p_completada)
    {
    /*PROTECTED REGION ID(ParadaActividadController_MarcarActividadCompletada_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.MarcarActividadCompletadaAsync(p_oid, p_completada);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_ReordenarHorario_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ReordenarHorario(int p_oid, int p_nuevoDia, DateTime p_nuevoHorario)
    {
    /*PROTECTED REGION ID(ParadaActividadController_ReordenarHorario_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.ReordenarHorarioAsync(p_oid, p_nuevoDia, p_nuevoHorario);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
