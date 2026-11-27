// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(ChecklistController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.UseCases.Checklist;
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

        // Vista: botón "Vaciar lista" en el detalle del checklist (solo el propietario).
        if (!await EsMiChecklistAsync(p_oid)) return NotFound();

        // customTransaction: se ejecuta con el caso de uso, dentro de una transacción.
        var unitOfWorkFactory = HttpContext.RequestServices.GetRequiredService<IUnitOfWorkFactory>();
        await using var unitOfWork = unitOfWorkFactory.Create();
        await new ChecklistUseCase(unitOfWork).VaciarChecklistAsync(p_oid);

        TempData["ChecklistMensaje"] = "Se han eliminado todas las tareas de la lista.";
        return RedirectToAction(nameof(Details), new { id = p_oid });
    /*PROTECTED REGION END*/
    }
}
