// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(RecordatorioController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(RecordatorioController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class RecordatorioController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_ProgramarAviso_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ProgramarAviso(int p_oid, DateTime p_fecha, DateTime p_hora, TipoAviso p_tipo)
    {
    /*PROTECTED REGION ID(RecordatorioController_ProgramarAviso_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _recordatorioService.ProgramarAvisoAsync(p_oid, p_fecha, p_hora, p_tipo);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_EnviarNotificacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> EnviarNotificacion(int p_oid)
    {
    /*PROTECTED REGION ID(RecordatorioController_EnviarNotificacion_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _recordatorioService.EnviarNotificacionAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
