// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ChecklistController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(ChecklistController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class ChecklistController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_VaciarChecklist_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> VaciarChecklist(int p_oid)
    {
    /*PROTECTED REGION ID(ChecklistController_VaciarChecklist_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _checklistService.VaciarChecklistAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
