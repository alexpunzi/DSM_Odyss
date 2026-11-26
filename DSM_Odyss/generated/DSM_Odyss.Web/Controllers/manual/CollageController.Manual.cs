// HARIA-WEB MVC managed manual-controller
/*PROTECTED REGION ID(CollageController_manual_usings) ENABLED START*/
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
/*PROTECTED REGION ID(CollageController_manual_class_attributes) ENABLED START*/
// Class-level authorization applies to both partial files.
// [Authorize(Roles = "Admin")]
/*PROTECTED REGION END*/
public partial class CollageController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CollageController_GenerarCollageAutomatico_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> GenerarCollageAutomatico(int p_viaje_oid, string? imagenDataUrl)
    {
    /*PROTECTED REGION ID(CollageController_GenerarCollageAutomatico_POST_body) ENABLED START*/

        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();
        if (!IsValidGeneratedJpeg(imagenDataUrl)) return BadRequest("La imagen del collage no es válida.");

        await using var unitOfWork = _unitOfWorkFactory.Create();
        var viaje = await unitOfWork.ViajeRepository.ReadAsync(p_viaje_oid);
        if (viaje is null) return NotFound();
        if (viaje.Carpetas?.Multimedia.Any(item => item.Tipo == TipoMultimedia.foto) != true)
            return BadRequest("El viaje todavía no tiene fotos para crear el collage.");

        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        if (cliente is null) return Forbid();
        var puedeEditar = cliente.Viajes.Any(item => item.Id == p_viaje_oid) ||
                          viaje.Carpetas?.CompartidoConEdicion.Any(item => item.Id == clienteId) == true;
        if (!puedeEditar) return Forbid();

        await unitOfWork.BeginTransactionAsync("MVC.Collage.Generate");
        try
        {
            await new CollageService(unitOfWork)
                .GenerarCollageAutomaticoAsync(p_viaje_oid, imagenDataUrl!);
            await unitOfWork.CommitAsync();
            return NoContent();
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "Could not generate the collage for trip {ViajeId}.", p_viaje_oid);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    /*PROTECTED REGION END*/
    }

    private static bool IsValidGeneratedJpeg(string? imageDataUrl)
    {
        const string prefix = "data:image/jpeg;base64,";
        const int maxEncodedLength = 2_500_000;
        if (imageDataUrl is null ||
            imageDataUrl.Length > maxEncodedLength ||
            !imageDataUrl.StartsWith(prefix, StringComparison.Ordinal))
            return false;

        try
        {
            var bytes = Convert.FromBase64String(imageDataUrl[prefix.Length..]);
            return bytes.Length >= 4 &&
                   bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF &&
                   bytes[^2] == 0xFF && bytes[^1] == 0xD9;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
