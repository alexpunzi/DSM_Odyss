// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(CarpetaController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(CarpetaController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class CarpetaController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_CompartirCarpeta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CompartirCarpeta(int p_oid, int p_cliente_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_CompartirCarpeta_POST_body) ENABLED START*/
        if (!TryGetCurrentClientId(out var currentClientId)) return Challenge();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, p_oid);
        if (context is null) return NotFound();
        if (!IsOwner(context, currentClientId)) return Forbid();
        if (context.OwnerIds.Contains(p_cliente_oid))
            return BadRequest("No se puede compartir una carpeta con su propietario.");

        await _carpetaService.CompartirCarpetaAsync(p_oid, p_cliente_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
