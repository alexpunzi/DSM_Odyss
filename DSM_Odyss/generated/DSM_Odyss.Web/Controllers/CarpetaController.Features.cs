using System.Security.Cryptography;
using System.Text;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSM_Odyss.Web.Controllers;

public partial class CarpetaController
{
    private const long MaxUploadBytes = 25 * 1024 * 1024;
    private static readonly IReadOnlyDictionary<string, TipoMultimedia> SupportedExtensions =
        new Dictionary<string, TipoMultimedia>(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = TipoMultimedia.foto,
            [".jpeg"] = TipoMultimedia.foto,
            [".png"] = TipoMultimedia.foto,
            [".gif"] = TipoMultimedia.foto,
            [".webp"] = TipoMultimedia.foto,
            [".mp4"] = TipoMultimedia.video,
            [".webm"] = TipoMultimedia.video,
            [".mov"] = TipoMultimedia.video
        };

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubirArchivo(int carpetaId, IFormFile? archivo)
    {
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, carpetaId);
        if (context is null) return NotFound();
        if (!CanEdit(context, clientId)) return Forbid();

        if (archivo is null || archivo.Length == 0)
        {
            TempData["CarpetaError"] = "Selecciona una foto o un vídeo para subir.";
            return RedirectToAction(nameof(Details), new { id = carpetaId });
        }

        if (archivo.Length > MaxUploadBytes)
        {
            TempData["CarpetaError"] = "El archivo supera el límite de 25 MB.";
            return RedirectToAction(nameof(Details), new { id = carpetaId });
        }

        var extension = Path.GetExtension(archivo.FileName);
        if (!SupportedExtensions.TryGetValue(extension, out var tipo))
        {
            TempData["CarpetaError"] = "Formato no admitido. Usa JPG, PNG, GIF, WEBP, MP4, WEBM o MOV.";
            return RedirectToAction(nameof(Details), new { id = carpetaId });
        }

        var uploadDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", "uploads");
        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadDirectory, fileName);

        try
        {
            Directory.CreateDirectory(uploadDirectory);
            await using (var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await archivo.CopyToAsync(fileStream, HttpContext.RequestAborted);
            }

            var media = new Multimedia
            {
                Tipo = tipo,
                Nombre = Path.GetFileName(archivo.FileName),
                FechaSubida = DateTime.Today,
                Descripcion = string.Empty,
                RutaArchivo = fileName
            };

            await unitOfWork.BeginTransactionAsync("MVC.Carpeta.Upload");
            context.Carpeta.Multimedia.Add(media);
            await unitOfWork.MultimediaRepository.SaveAsync(media);
            await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
            await unitOfWork.CommitAsync();
            if (tipo == TipoMultimedia.foto)
                TempData["GenerarCollage"] = true;
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            TryDeleteUploadedFile(fileName);
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "Could not upload media to folder {CarpetaId}.", carpetaId);
            TempData["CarpetaError"] = "No se pudo guardar el archivo. Inténtalo de nuevo.";
        }

        return RedirectToAction(nameof(Details), new { id = carpetaId });
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Media(int id, string? token)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var media = await unitOfWork.MultimediaRepository.ReadAsync(id);
        if (media is null || string.IsNullOrWhiteSpace(media.RutaArchivo)) return NotFound();

        var folders = await unitOfWork.CarpetaRepository.ReadAllAsync();
        var parent = folders.FirstOrDefault(folder => folder.Multimedia.Any(item => item.Id == id));
        if (parent is null) return NotFound();

        var context = await LoadCarpetaContextAsync(unitOfWork, parent.Id);
        if (context is null) return NotFound();
        var clientId = TryGetCurrentClientId(out var currentClientId) ? currentClientId : (int?)null;
        var canView = (clientId.HasValue &&
                       (IsOwner(context, clientId.Value) ||
                        context.Carpeta.CompartidoCon.Any(cliente => cliente.Id == clientId.Value)))
                      || HasValidShareToken(token, context.Carpeta.PublicShareToken);
        if (!canView) return NotFound();

        var fileName = Path.GetFileName(media.RutaArchivo);
        if (!string.Equals(fileName, media.RutaArchivo, StringComparison.Ordinal)) return NotFound();
        var path = Path.Combine(_environment.ContentRootPath, "App_Data", "uploads", fileName);
        if (!System.IO.File.Exists(path)) return NotFound();

        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        var contentType = GetContentType(Path.GetExtension(fileName));
        return PhysicalFile(path, contentType, enableRangeProcessing: media.Tipo == TipoMultimedia.video);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarPermiso(int carpetaId, int clienteId, bool puedeEditar)
    {
        if (!TryGetCurrentClientId(out var currentClientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, carpetaId);
        if (context is null) return NotFound();
        if (!IsOwner(context, currentClientId)) return Forbid();

        var target = context.Clientes.FirstOrDefault(cliente => cliente.Id == clienteId);
        if (target is null) return BadRequest("El usuario indicado no existe.");
        if (context.OwnerIds.Contains(clienteId)) return BadRequest("No se puede compartir una carpeta con su propietario.");

        context.Carpeta.CompartidoCon ??= new List<Cliente>();
        context.Carpeta.CompartidoConEdicion ??= new List<Cliente>();
        AddIfMissing(context.Carpeta.CompartidoCon, target);
        AddIfMissing(target.CarpetasCompartidas, context.Carpeta);

        if (puedeEditar)
            AddIfMissing(context.Carpeta.CompartidoConEdicion, target);
        else
            context.Carpeta.CompartidoConEdicion.Remove(target);

        await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        return RedirectToAction(nameof(Details), new { id = carpetaId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarCompartido(int carpetaId, int clienteId)
    {
        if (!TryGetCurrentClientId(out var currentClientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, carpetaId);
        if (context is null) return NotFound();
        if (!IsOwner(context, currentClientId)) return Forbid();

        var target = context.Clientes.FirstOrDefault(cliente => cliente.Id == clienteId);
        if (target is null) return BadRequest("El usuario indicado no existe.");
        context.Carpeta.CompartidoCon.Remove(target);
        context.Carpeta.CompartidoConEdicion.Remove(target);
        target.CarpetasCompartidas.Remove(context.Carpeta);
        await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        return RedirectToAction(nameof(Details), new { id = carpetaId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RevocarEnlace(int carpetaId)
    {
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, carpetaId);
        if (context is null) return NotFound();
        if (!IsOwner(context, clientId)) return Forbid();

        context.Carpeta.PublicShareToken = CreateShareToken();
        await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        return RedirectToAction(nameof(Details), new { id = carpetaId });
    }

    private async Task<CarpetaContext?> LoadCarpetaContextAsync(IUnitOfWork unitOfWork, int id)
    {
        var carpeta = await unitOfWork.CarpetaRepository.ReadAsync(id);
        if (carpeta is null) return null;

        var viajes = (await unitOfWork.ViajeRepository.ReadAllAsync())
            .Where(viaje => viaje.Carpetas?.Id == id)
            .ToList();
        var viajeIds = viajes.Select(viaje => viaje.Id).ToHashSet();
        var clientes = (await unitOfWork.ClienteRepository.ReadAllAsync()).ToList();
        var ownerIds = clientes
            .Where(cliente => cliente.Viajes.Any(viaje => viajeIds.Contains(viaje.Id)))
            .Select(cliente => cliente.Id)
            .ToHashSet();

        return new CarpetaContext(carpeta, viajes, clientes, ownerIds);
    }

    private async Task<HashSet<int>> GetAccessibleFolderIdsAsync(IUnitOfWork unitOfWork, int clientId)
    {
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clientId);
        if (cliente is null) return new HashSet<int>();

        return cliente.Viajes
            .Where(viaje => viaje.Carpetas is not null)
            .Select(viaje => viaje.Carpetas!.Id)
            .Concat(cliente.CarpetasCompartidas.Select(carpeta => carpeta.Id))
            .ToHashSet();
    }

    private async Task<bool> IsOwnerOfTripAsync(IUnitOfWork unitOfWork, int viajeId, int clientId)
    {
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clientId);
        return cliente?.Viajes.Any(viaje => viaje.Id == viajeId) == true;
    }

    private static bool IsOwner(CarpetaContext context, int clientId) =>
        context.OwnerIds.Contains(clientId);

    private static bool CanEdit(CarpetaContext context, int clientId) =>
        IsOwner(context, clientId) ||
        context.Carpeta.CompartidoConEdicion.Any(cliente => cliente.Id == clientId);

    private bool TryGetCurrentClientId(out int clientId) =>
        int.TryParse(User.Identity?.Name, out clientId);

    private static bool HasValidShareToken(string? provided, string? stored)
    {
        if (string.IsNullOrEmpty(provided) || string.IsNullOrEmpty(stored)) return false;
        var providedBytes = Encoding.UTF8.GetBytes(provided);
        var storedBytes = Encoding.UTF8.GetBytes(stored);
        return providedBytes.Length == storedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(providedBytes, storedBytes);
    }

    private static string CreateShareToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

    private static string GetContentType(string extension) =>
        extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".mov" => "video/quicktime",
            _ => "application/octet-stream"
        };

    private static void AddIfMissing<T>(ICollection<T> collection, T item)
    {
        if (!collection.Contains(item)) collection.Add(item);
    }

    private void TryDeleteUploadedFile(string fileName)
    {
        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName)) return;
        var path = Path.Combine(_environment.ContentRootPath, "App_Data", "uploads", safeFileName);
        try
        {
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
        catch (IOException error)
        {
            _logger.LogWarning(error, "Could not remove uploaded file {FileName}.", safeFileName);
        }
        catch (UnauthorizedAccessException error)
        {
            _logger.LogWarning(error, "Could not remove uploaded file {FileName}.", safeFileName);
        }
    }

    private sealed record CarpetaContext(
        Carpeta Carpeta,
        IReadOnlyList<Viaje> Viajes,
        IReadOnlyList<Cliente> Clientes,
        HashSet<int> OwnerIds);
}
