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

        // Vista: casilla de cada actividad en el detalle del viaje (solo el autor).
        var viaje = await ViajeDeActividadPropiaAsync(p_oid);
        if (viaje is null) return Forbid();

        await _paradaActividadService.MarcarActividadCompletadaAsync(p_oid, p_completada);
        return RedirectToAction("Details", "Viaje", new { id = viaje.Id });
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_ReordenarHorario_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ReordenarHorario(int p_oid, int p_nuevoDia, DateTime p_nuevoHorario)
    {
    /*PROTECTED REGION ID(ParadaActividadController_ReordenarHorario_POST_body) ENABLED START*/

        // Vista: formulario "Cambiar día y hora" de cada actividad en el detalle del viaje (solo el autor).
        var viaje = await ViajeDeActividadPropiaAsync(p_oid);
        if (viaje is null) return Forbid();

        var totalDias = (viaje.FechaFin.Date - viaje.FechaInicio.Date).Days + 1;
        if (!ModelState.IsValid || p_nuevoDia < 1 || p_nuevoDia > totalDias)
        {
            TempData["ViajeError"] = $"Elige un día entre 1 y {totalDias} y una hora válida.";
            return RedirectToAction("Details", "Viaje", new { id = viaje.Id });
        }

        // La fecha de la actividad es la del día elegido; del formulario solo nos interesa la hora.
        var nuevoHorario = viaje.FechaInicio.Date.AddDays(p_nuevoDia - 1).Add(p_nuevoHorario.TimeOfDay);
        await _paradaActividadService.ReordenarHorarioAsync(p_oid, p_nuevoDia, nuevoHorario);
        TempData["ViajeOk"] = "Actividad movida.";
        return RedirectToAction("Details", "Viaje", new { id = viaje.Id });
    /*PROTECTED REGION END*/
    }
}
