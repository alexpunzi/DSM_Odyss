// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(CarpetaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(CarpetaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class CarpetaController : Controller
{
    /*PROTECTED REGION ID(CarpetaController_dependencies) START*/
    private readonly CarpetaService _carpetaService;
    private readonly ILogger<CarpetaController> _logger;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IWebHostEnvironment _environment;
    public CarpetaController(
        CarpetaService carpetaService,
        IUnitOfWorkFactory unitOfWorkFactory,
        IWebHostEnvironment environment,
        ILogger<CarpetaController>? logger = null)
    {
        _carpetaService = carpetaService;
        _unitOfWorkFactory = unitOfWorkFactory;
        _environment = environment;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<CarpetaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(CarpetaController_Index_GET_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();

        await using var unitOfWork = _unitOfWorkFactory.Create();
        var client = await unitOfWork.ClienteRepository.ReadAsync(clientId);
        if (client is null) return Forbid();

        var viajes = (await unitOfWork.ViajeRepository.ReadAllAsync())
            .Where(viaje => viaje.Carpetas is not null)
            .ToList();
        var viajePorCarpeta = viajes
            .GroupBy(viaje => viaje.Carpetas!.Id)
            .ToDictionary(group => group.Key, group => group.First());
        var carpetasPropias = client.Viajes
            .Where(viaje => viaje.Carpetas is not null)
            .Select(viaje => viaje.Carpetas!.Id)
            .ToHashSet();
        var carpetasCompartidas = client.CarpetasCompartidas
            .Select(carpeta => carpeta.Id)
            .ToHashSet();

        var entities = await unitOfWork.CarpetaRepository.ReadAllAsync();
        var model = entities
            .Where(carpeta => carpetasPropias.Contains(carpeta.Id) || carpetasCompartidas.Contains(carpeta.Id))
            .Select(carpeta =>
            {
                viajePorCarpeta.TryGetValue(carpeta.Id, out var viaje);
                return new DSM_Odyss.Web.ViewModels.CarpetaViewModel
                {
                    Id = carpeta.Id,
                    Nombre = carpeta.Nombre,
                    TotalArchivos = carpeta.Multimedia.Count,
                    ViajeId = viaje?.Id ?? 0,
                    ViajeTitulo = viaje?.Titulo ?? "Carpeta compartida"
                };
            })
            .ToList();
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    [AllowAnonymous]
    /*PROTECTED REGION ID(CarpetaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id, string? token, string? q)
    {
    /*PROTECTED REGION ID(CarpetaController_Details_GET_body) START*/
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, id);
        if (context is null) return NotFound();

        var clientId = TryGetCurrentClientId(out var currentClientId) ? currentClientId : (int?)null;
        var isOwner = clientId.HasValue && context.OwnerIds.Contains(clientId.Value);
        var isShared = clientId.HasValue && context.Carpeta.CompartidoCon.Any(cliente => cliente.Id == clientId.Value);
        var isPublicLink = HasValidShareToken(token, context.Carpeta.PublicShareToken);
        if (!isOwner && !isShared && !isPublicLink) return NotFound();

        var canEdit = isOwner || (clientId.HasValue &&
            context.Carpeta.CompartidoConEdicion.Any(cliente => cliente.Id == clientId.Value));
        var viaje = context.Viajes.FirstOrDefault();
        if (isOwner && string.IsNullOrWhiteSpace(context.Carpeta.PublicShareToken))
        {
            context.Carpeta.PublicShareToken = CreateShareToken();
            await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        }

        var mediaToken = isPublicLink ? token : null;
        var photoFiles = context.Carpeta.Multimedia
            .Where(item => item.Tipo == TipoMultimedia.foto)
            .ToList();
        var hasGeneratedCollage = viaje?.Collages?.UrlImagen?.StartsWith(
            "data:image/jpeg;base64,", StringComparison.Ordinal) == true;
        var generateCollage = canEdit &&
                              viaje is not null &&
                              photoFiles.Count > 0 &&
                              (TempData["GenerarCollage"] as bool? == true || !hasGeneratedCollage);
        var model = new DSM_Odyss.Web.ViewModels.CarpetaDetailsViewModel
        {
            Id = context.Carpeta.Id,
            Nombre = context.Carpeta.Nombre,
            ViajeId = viaje?.Id ?? 0,
            ViajeTitulo = viaje?.Titulo ?? string.Empty,
            CollageUrl = viaje?.Collages?.UrlImagen,
            AutoGenerateCollage = generateCollage,
            PuedeEditar = canEdit,
            EsPropietario = isOwner,
            EsEnlacePublico = isPublicLink && !canEdit,
            EnlacePublico = isOwner
                ? Url.Action(nameof(Details), "Carpeta", new { id, token = context.Carpeta.PublicShareToken }, Request.Scheme)
                : null,
            Busqueda = q,
            Archivos = context.Carpeta.Multimedia
                .OrderByDescending(item => item.FechaSubida)
                .Select(item => new DSM_Odyss.Web.ViewModels.CarpetaMultimediaItemViewModel
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    EsVideo = item.Tipo == TipoMultimedia.video,
                    TieneArchivo = !string.IsNullOrWhiteSpace(item.RutaArchivo),
                    Url = Url.Action(nameof(Media), "Carpeta", new { id = item.Id, token = mediaToken }) ?? string.Empty
                })
                .ToList(),
            Compartidos = isOwner
                ? context.Carpeta.CompartidoCon
                    .Select(cliente => new DSM_Odyss.Web.ViewModels.CarpetaCompartidoItemViewModel
                    {
                        Id = cliente.Id,
                        Usuario = cliente.User,
                        PuedeEditar = context.Carpeta.CompartidoConEdicion.Any(editor => editor.Id == cliente.Id)
                    })
                    .OrderBy(item => item.Usuario)
                    .ToList()
                : new()
        };

        if (isOwner && !string.IsNullOrWhiteSpace(q) && q.Trim().Length >= 2)
        {
            var query = q.Trim();
            var existingIds = context.Carpeta.CompartidoCon.Select(cliente => cliente.Id).ToHashSet();
            model.ResultadosBusqueda = context.Clientes
                .Where(cliente =>
                    !context.OwnerIds.Contains(cliente.Id) &&
                    !existingIds.Contains(cliente.Id) &&
                    (cliente.User.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     cliente.Nombre.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     cliente.Apellidos.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Take(10)
                .Select(cliente => new DSM_Odyss.Web.ViewModels.CarpetaUsuarioItemViewModel
                {
                    Id = cliente.Id,
                    Usuario = cliente.User,
                    NombreCompleto = $"{cliente.Nombre} {cliente.Apellidos}".Trim()
                })
                .ToList();
        }

        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(int viajeId)
    {
    /*PROTECTED REGION ID(CarpetaController_Create_GET_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var viaje = await unitOfWork.ViajeRepository.ReadAsync(viajeId);
        if (viaje is null) return NotFound();
        if (!await IsOwnerOfTripAsync(unitOfWork, viajeId, clientId)) return Forbid();
        if (viaje.Carpetas is not null)
            return RedirectToAction(nameof(Details), new { id = viaje.Carpetas.Id });

        return View(new DSM_Odyss.Web.ViewModels.CarpetaViewModel
        {
            ViajeId = viajeId,
            ViajeTitulo = viaje.Titulo,
            Nombre = $"Recuerdos de {viaje.Titulo}"
        });
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.CarpetaViewModel model)
    {
    /*PROTECTED REGION ID(CarpetaController_Create_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        if (!ModelState.IsValid) return View(model);

        await using var unitOfWork = _unitOfWorkFactory.Create();
        var viaje = await unitOfWork.ViajeRepository.ReadAsync(model.ViajeId);
        if (viaje is null) return NotFound();
        if (!await IsOwnerOfTripAsync(unitOfWork, model.ViajeId, clientId)) return Forbid();
        if (viaje.Carpetas is not null)
            return RedirectToAction(nameof(Details), new { id = viaje.Carpetas.Id });

        try
        {
            var carpeta = await new CarpetaService(unitOfWork).CrearCarpetaAsync(model.Nombre, model.ViajeId);
            return RedirectToAction(nameof(Details), new { id = carpeta.Id });
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not create the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            model.ViajeTitulo = viaje.Titulo;
            return View(model);
        }
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_Edit_GET_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, id);
        if (context is null) return NotFound();
        if (!CanEdit(context, clientId)) return Forbid();
        return View(CarpetaAssembler.ToViewModel(context.Carpeta));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.CarpetaViewModel model)
    {
    /*PROTECTED REGION ID(CarpetaController_Edit_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, id);
        if (context is null) return NotFound();
        if (!CanEdit(context, clientId)) return Forbid();

        if (!ModelState.IsValid) return View(model);

        try
        {
            context.Carpeta.Nombre = model.Nombre;
            await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not update the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }
        return RedirectToAction(nameof(Details), new { id });
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_Delete_GET_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, id);
        if (context is null) return NotFound();
        if (!IsOwner(context, clientId)) return Forbid();
        return View(CarpetaAssembler.ToViewModel(context.Carpeta));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_DeleteConfirmed_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, id);
        if (context is null) return NotFound();
        if (!IsOwner(context, clientId)) return Forbid();

        var uploadedFiles = context.Carpeta.Multimedia
            .Select(item => item.RutaArchivo)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .ToList();
        await unitOfWork.BeginTransactionAsync("MVC.Carpeta.Delete");
        try
        {
            foreach (var viaje in context.Viajes)
            {
                viaje.Carpetas = null;
                await unitOfWork.ViajeRepository.SaveAsync(viaje);
            }
            await unitOfWork.CarpetaRepository.DeleteAsync(context.Carpeta);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        foreach (var uploadedFile in uploadedFiles) TryDeleteUploadedFile(uploadedFile);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_ConMultimedia_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConMultimedia()
    {
    /*PROTECTED REGION ID(CarpetaController_ConMultimedia_GET_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var allowedIds = await GetAccessibleFolderIdsAsync(unitOfWork, clientId);
        var result = await unitOfWork.CarpetaRepository.ConMultimediaAsync();
        return Ok(result
            .Where(carpeta => allowedIds.Contains(carpeta.Id))
            .Select(CarpetaAssembler.ToViewModel)
            .ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_AgregarMultimedia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarMultimedia(int p_oid, IReadOnlyList<int> p_multimedia_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_AgregarMultimedia_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, p_oid);
        if (context is null) return NotFound();
        if (!CanEdit(context, clientId)) return Forbid();
        await _carpetaService.AgregarMultimediaAsync(p_oid, p_multimedia_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_CompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_CompartirConCliente_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, p_oid);
        if (context is null) return NotFound();
        if (!IsOwner(context, clientId)) return Forbid();
        foreach (var targetId in p_clientes_oid)
        {
            var target = context.Clientes.FirstOrDefault(cliente => cliente.Id == targetId);
            if (target is null) return BadRequest("El usuario indicado no existe.");
            if (context.OwnerIds.Contains(targetId)) return BadRequest("No se puede compartir una carpeta con su propietario.");
            AddIfMissing(context.Carpeta.CompartidoCon, target);
            context.Carpeta.CompartidoConEdicion.Remove(target);
        }
        await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_DejarDeCompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DejarDeCompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_DejarDeCompartirConCliente_POST_body) START*/
        if (!TryGetCurrentClientId(out var clientId)) return Challenge();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var context = await LoadCarpetaContextAsync(unitOfWork, p_oid);
        if (context is null) return NotFound();
        if (!IsOwner(context, clientId)) return Forbid();
        foreach (var targetId in p_clientes_oid)
        {
            var target = context.Clientes.FirstOrDefault(cliente => cliente.Id == targetId);
            if (target is null) return BadRequest("El usuario indicado no existe.");
            context.Carpeta.CompartidoCon.Remove(target);
            context.Carpeta.CompartidoConEdicion.Remove(target);
            target.CarpetasCompartidas.Remove(context.Carpeta);
        }
        await unitOfWork.CarpetaRepository.SaveAsync(context.Carpeta);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
