// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(TareaController_manual_usings) ENABLED START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
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

        // Vista: casilla de cada tarea en el detalle del checklist (solo el propietario).
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();
        var clienteService = HttpContext.RequestServices.GetRequiredService<ClienteService>();
        var cliente = await clienteService.LeerClienteOIDAsync(clienteId);
        var checklistId = cliente?.Checklists
            .FirstOrDefault(c => c.Tareas.Any(t => t.Id == p_oid))?.Id;
        if (checklistId is null) return Forbid();

        await _tareaService.CambiarEstadoTareaAsync(p_oid, p_completada);
        return RedirectToAction("Details", "Checklist", new { id = checklistId });
    /*PROTECTED REGION END*/
    }
}
