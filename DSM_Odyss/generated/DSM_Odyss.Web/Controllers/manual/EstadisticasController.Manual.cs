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

        // Vista: botón "Recalcular" en la página de estadísticas (solo sus propias estadísticas).
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();
        await using var unitOfWork = _useCaseUnitOfWorkFactory.Create();
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        var estadisticas = cliente?.Estadisticas.FirstOrDefault(e => e.Id == p_oid);
        if (estadisticas is null) return Forbid();
        var anyo = estadisticas.Anyo;

        // customTransaction: se ejecuta con el caso de uso, dentro de una transacción.
        var useCase = new DSM_Odyss.Application.UseCases.Estadisticas.EstadisticasUseCase(unitOfWork);
        await useCase.RecalcularEstadisticasAsync(p_oid);

        TempData["EstadisticasMensaje"] = "Estadísticas recalculadas con tus viajes actuales.";
        return RedirectToAction(nameof(Index), new { anyo });
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
