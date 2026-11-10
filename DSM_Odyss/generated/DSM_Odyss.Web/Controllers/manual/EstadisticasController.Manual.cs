// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(EstadisticasController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(EstadisticasController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class EstadisticasController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_RecalcularEstadisticas_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> RecalcularEstadisticas(int p_oid)
    {
    /*PROTECTED REGION ID(EstadisticasController_RecalcularEstadisticas_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var useCase = new DSM_Odyss.Application.UseCases.Estadisticas.EstadisticasUseCase(unitOfWork);
        await useCase.RecalcularEstadisticasAsync(p_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_GenerarPassportRecap_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> GenerarPassportRecap(int p_oid, int p_anyo)
    {
    /*PROTECTED REGION ID(EstadisticasController_GenerarPassportRecap_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _estadisticasService.GenerarPassportRecapAsync(p_oid, p_anyo);
        return Ok(result);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_ObtenerPaisesColoreados_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ObtenerPaisesColoreados(int p_oid)
    {
    /*PROTECTED REGION ID(EstadisticasController_ObtenerPaisesColoreados_POST_body) ENABLED START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _estadisticasService.ObtenerPaisesColoreadosAsync(p_oid);
        return Ok(result);
    /*PROTECTED REGION END*/
    }
}
