// Código manual (no generado por HARIA): vistas de las operaciones custom y customTransaction de Viaje.
//   - publicarItinerario   (Custom)            → botón "Hacer público / privado" en el detalle del viaje
//   - cambiarEstadoViaje   (Custom)            → desplegable de estado en el detalle del viaje
//   - duplicarItinerario   (CustomTransaction) → botón "Duplicar viaje" en el detalle del viaje (ViajeUseCase)
// compartirItinerario (Custom) se usa desde ViajeController.Compartir.cs.
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Application.UseCases.Viaje;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Web.Controllers;

public partial class ViajeController
{
    // POST /Viaje/PublicarItinerario/5 — hace público o privado un viaje propio.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PublicarItinerario(int id, bool esPublico)
    {
        if (!await EsAutorDelViajeAsync(id)) return Forbid();

        await _viajeService.PublicarItinerarioAsync(id, esPublico);
        TempData["ViajeOk"] = esPublico
            ? "El viaje ahora es público: aparece en tu perfil y en la Home."
            : "El viaje ahora es privado: solo lo ves tú y las personas con quienes lo compartas.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // POST /Viaje/CambiarEstadoViaje/5 — planificado / en curso / finalizado.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstadoViaje(int id, EstadoViaje nuevoEstado)
    {
        if (!await EsAutorDelViajeAsync(id)) return Forbid();

        try
        {
            await _viajeService.CambiarEstadoViajeAsync(id, nuevoEstado);
            TempData["ViajeOk"] = "Estado del viaje actualizado.";
        }
        catch (ModelException)
        {
            TempData["ViajeError"] = "Ese estado no es válido.";
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    // POST /Viaje/DuplicarItinerario/5 — crea una copia privada del viaje (customTransaction).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DuplicarItinerario(int id, string? nuevoTitulo)
    {
        if (!await EsAutorDelViajeAsync(id)) return Forbid();

        if (string.IsNullOrWhiteSpace(nuevoTitulo))
        {
            TempData["ViajeError"] = "Escribe un título para la copia del viaje.";
            return RedirectToAction(nameof(Details), new { id });
        }

        try
        {
            await using var unitOfWork = _formUnitOfWorkFactory.Create();
            var copia = await new ViajeUseCase(unitOfWork).DuplicarItinerarioAsync(id, nuevoTitulo.Trim());

            TempData["ViajeOk"] = "Viaje duplicado. Esta es la copia (privada y planificada).";
            return RedirectToAction(nameof(Details), new { id = copia.Id });
        }
        catch (ModelException error)
        {
            _logger.LogError(error, "No se pudo duplicar el viaje {ViajeId}.", id);
            TempData["ViajeError"] = "No se ha podido duplicar el viaje. Inténtalo de nuevo.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
