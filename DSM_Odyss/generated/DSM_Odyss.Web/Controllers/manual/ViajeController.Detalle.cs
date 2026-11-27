// Código manual (no generado por HARIA): borrar un viaje propio y reseñar viajes ajenos.
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Controllers;

public partial class ViajeController
{
    // POST /Viaje/Borrar/5 — solo el autor puede borrar su viaje.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrar(int id)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        var viaje = cliente?.Viajes.FirstOrDefault(v => v.Id == id);
        if (cliente is null || viaje is null) return Forbid();   // no existe o no es tuyo

        var carpeta = viaje.Carpetas;
        var collage = viaje.Collages;

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Borrar");
        try
        {
            // 1. Lo que depende del viaje.
            foreach (var actividad in viaje.Paradas.ToList())
                await unitOfWork.ParadaActividadRepository.DeleteAsync(actividad);
            foreach (var parada in viaje.ParadasDestino.ToList())
                await unitOfWork.ParadaDestinoRepository.DeleteAsync(parada);
            foreach (var resena in viaje.Resenas.ToList())
                await unitOfWork.ResenaRepository.DeleteAsync(resena);

            // 2. Quitarlo de la lista de viajes del autor y borrarlo.
            cliente.Viajes.Remove(viaje);
            await unitOfWork.ClienteRepository.SaveAsync(cliente);
            await unitOfWork.ViajeRepository.DeleteAsync(viaje);

            // 3. Su carpeta (con sus archivos) y su collage.
            if (carpeta is not null) await unitOfWork.CarpetaRepository.DeleteAsync(carpeta);
            if (collage is not null) await unitOfWork.CollageRepository.DeleteAsync(collage);

            await unitOfWork.CommitAsync();
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "No se pudo borrar el viaje {ViajeId}.", id);
            TempData["ViajeError"] = "No se ha podido borrar el viaje. Inténtalo de nuevo.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Vuelve a la lista de donde venía el viaje.
        return viaje.EsPublico
            ? RedirectToAction("Details", "Cliente")
            : RedirectToAction(nameof(Index));
    }

    // POST /Viaje/Resenar/5 — reseñar un viaje público de otra persona.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Resenar(int id, int puntuacion, string? comentario)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();

        if (puntuacion < 1 || puntuacion > 5)
        {
            TempData["ResenaError"] = "Elige una puntuación de 1 a 5 estrellas.";
            return RedirectToAction(nameof(Details), new { id });
        }

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var viaje = await unitOfWork.ViajeRepository.ReadAsync(id);
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        if (viaje is null || cliente is null || !viaje.EsPublico) return NotFound();

        if (cliente.Viajes.Any(v => v.Id == id))
        {
            TempData["ResenaError"] = "No puedes reseñar tu propio viaje.";
            return RedirectToAction(nameof(Details), new { id });
        }
        if (cliente.Resenas.Any(r => viaje.Resenas.Any(x => x.Id == r.Id)))
        {
            TempData["ResenaError"] = "Ya has reseñado este viaje.";
            return RedirectToAction(nameof(Details), new { id });
        }

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Resenar");
        try
        {
            var resena = new Resena
            {
                Comentario = (comentario ?? string.Empty).Trim(),
                Fecha = DateTime.Today,
                Puntuacion = puntuacion
            };
            await unitOfWork.ResenaRepository.SaveAsync(resena);

            // CrearResenaAsync no enlaza la reseña, así que lo hacemos aquí:
            viaje.Resenas.Add(resena);     // la reseña pertenece al viaje
            cliente.Resenas.Add(resena);   // y a su autor (sale en "Reseñas recientes" de su perfil)
            await unitOfWork.ViajeRepository.SaveAsync(viaje);
            await unitOfWork.ClienteRepository.SaveAsync(cliente);

            await unitOfWork.CommitAsync();
            TempData["ResenaOk"] = "¡Gracias! Tu reseña se ha publicado.";
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "No se pudo guardar la reseña del viaje {ViajeId}.", id);
            TempData["ResenaError"] = "No se ha podido publicar la reseña. Inténtalo de nuevo.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}