// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ViajeController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;
using DSM_Odyss.Web.ViewModels;
using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ViajeController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ViajeController : Controller
{
    /*PROTECTED REGION ID(ViajeController_dependencies) START*/
    private readonly ViajeService _viajeService;
    private readonly ILogger<ViajeController> _logger;
    private readonly DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public ViajeController(ViajeService viajeService, DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<ViajeController>? logger = null)
    {
        _viajeService = viajeService;
        _formUnitOfWorkFactory = formUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ViajeController>.Instance;
    }
    /*PROTECTED REGION END*/

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ViajeController_Index_GET_body) START*/
        var entities = await _viajeService.LeerTodosViajesAsync();
        return View(entities.Select(ViajeAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ViajeController_Details_GET_body) START*/
        var entity = await _viajeService.LeerViajeOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ViajeAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(ViajeController_Create_GET_body) START*/
        var hoy = DateTime.Today;
        var fin = hoy.AddDays(3);

        var model = new ViajeViewModel
        {
            FechaInicio = hoy,
            FechaFin = fin,
            Destinos = new List<DestinoParadaItemViewModel>
            {
                new() { Orden = 1, FechaLlegada = hoy, FechaSalida = fin}
            },
            Paradas = new List<ParadaItemViewModel>
            {
                new() { Dia = 1 }
            }
        };

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(ViajeViewModel model, int p_autor_oid)
    {
    /*PROTECTED REGION ID(ViajeController_Create_POST_body) START*/
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ViajeService(unitOfWork);
        var destinoService = new DestinoService(unitOfWork);
        var paradaDestinoService = new ParadaDestinoService(unitOfWork);
        var paradaActividadService = new ParadaActividadService(unitOfWork);

        await LoadFormOptionsAsync(model, unitOfWork);

        // Resolver autor automáticamente si no viene en el formulario
        if (p_autor_oid <= 0)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
            var clientes = await unitOfWork.ClienteRepository.ReadAllAsync();
            var clienteActual = clientes.FirstOrDefault(c => c.Email == email) ?? clientes.FirstOrDefault();
            if (clienteActual != null) p_autor_oid = clienteActual.Id;
        }

        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Create");
        try
        {
            // Crear el viaje
            var entity = await service.CrearViajeAsync(
                model.Titulo,
                model.FechaInicio,
                model.FechaFin,
                model.EsPublico,
                model.Presupuesto,
                model.TipoViaje,
                model.Estado,
                p_autor_oid,
                model.CarpetasId.GetValueOrDefault()
            );

            if (model.CarpetasId is int referenceCarpetas)
            {
                entity.Carpetas = await unitOfWork.CarpetaRepository.ReadAsync(referenceCarpetas)
                    ?? throw new InvalidOperationException("El registro seleccionado de Carpeta ya no existe.");
            }
            if (model.CollagesId is int referenceCollages)
            {
                entity.Collages = await unitOfWork.CollageRepository.ReadAsync(referenceCollages)
                    ?? throw new InvalidOperationException("El registro seleccionado de Collage ya no existe.");
            }

            await service.ModifierViajeDefaultAsync(entity);

            // Procesar los destinos y crear cada ParadaDestino ordenada
            var catalogoDestinos = await unitOfWork.DestinoRepository.ReadAllAsync();
            var paradasDestinoCreadas = new Dictionary<int, int>(); // Diccionario para mapear el índice del destino al ID de la ParadaDestino creada
            int ordenActual = 1;

            for (int i = 0; i < model.Destinos.Count; i++)
            {
                var itemDestino = model.Destinos[i];
                if (string.IsNullOrWhiteSpace(itemDestino.Ciudad) && !itemDestino.DestinoId.HasValue)
                {
                    // Saltar destinos vacíos
                    continue;
                }

                int destinoOid;
                if (itemDestino.DestinoId.HasValue && itemDestino.DestinoId.Value > 0)
                {
                    destinoOid = itemDestino.DestinoId.Value;
                }
                else
                {
                    var ciudadNorm = itemDestino.Ciudad.Trim();
                    var paisNorm = string.IsNullOrWhiteSpace(itemDestino.Pais) ? "Sin especificar" : itemDestino.Pais.Trim();

                    var existente = catalogoDestinos.FirstOrDefault(d =>
                        (!string.IsNullOrWhiteSpace(ciudadNorm) && string.Equals(d.Ciudad, ciudadNorm, StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(d.Pais, paisNorm, StringComparison.OrdinalIgnoreCase)) ||
                        (string.IsNullOrWhiteSpace(ciudadNorm) && string.Equals(d.Pais, paisNorm, StringComparison.OrdinalIgnoreCase))
                    );

                    if (existente != null)
                    {
                        destinoOid = existente.Id;
                    }
                    else
                    {
                        var nuevoDestino = await destinoService.CrearDestinoAsync(paisNorm);
                        if (!string.IsNullOrWhiteSpace(ciudadNorm))
                        {
                            nuevoDestino.Ciudad = ciudadNorm;
                            await destinoService.ModifierDestinoDefaultAsync(nuevoDestino);
                        }
                        destinoOid = nuevoDestino.Id;
                    }
                }

                // Crear ParadaDestino enlazada al Viaje y al Destino con orden único
                var paradaDestino = await paradaDestinoService.CrearParadaDestinoAsync(
                    ordenActual,
                    itemDestino.FechaLlegada,
                    itemDestino.FechaSalida,
                    entity.Id,
                    destinoOid
                );

                paradasDestinoCreadas[i] = paradaDestino.Id;
                ordenActual++;
            }

            // Procesar las actividades (ParadaActividad)
            foreach (var act in model.Paradas.Where(p => !string.IsNullOrWhiteSpace(p.Nombre)))
            {
                var fechaActividad = model.FechaInicio.Date.AddDays(Math.Max(0, act.Dia - 1));
                DateTime horarioDt = fechaActividad;

                if (!string.IsNullOrWhiteSpace(act.Horario) && TimeSpan.TryParse(act.Horario, out var horaSpan))
                {
                    horarioDt = fechaActividad.Add(horaSpan);
                }
                else if (!string.IsNullOrWhiteSpace(act.Horario) && DateTime.TryParse(act.Horario, out var parsedDt))
                {
                    horarioDt = fechaActividad.Add(parsedDt.TimeOfDay);
                }

                await paradaActividadService.CrearParadaActividadAsync(
                    act.Nombre.Trim(),
                    act.Dia,
                    horarioDt,
                    false,
                    entity.Id
                );
            }

            await unitOfWork.CommitAsync();
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "No se pudo guardar el viaje con sus destinos y actividades.");
            ModelState.AddModelError(string.Empty, "No se han podido guardar el viaje y sus destinos.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ViajeController_Edit_GET_body) START*/
        var entity = await _viajeService.LeerViajeOIDAsync(id);
        if (entity is null) return NotFound();
        var model = ViajeAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, ViajeViewModel model, int p_autor_oid)
    {
    /*PROTECTED REGION ID(ViajeController_Edit_POST_body) START*/
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ViajeService(unitOfWork);
        var entity = await service.ReadOIDViajeDefaultAsync(id);
        if (entity is null) return NotFound();

        await LoadFormOptionsAsync(model, unitOfWork);

        if (p_autor_oid <= 0)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
            var clientes = await unitOfWork.ClienteRepository.ReadAllAsync();
            var clienteActual = clientes.FirstOrDefault(c => c.Email == email) ?? clientes.FirstOrDefault();
            if (clienteActual != null) p_autor_oid = clienteActual.Id;
        }

        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Edit");
        try
        {
            await service.ModificarViajeAsync(
                id,
                model.Titulo,
                model.FechaInicio,
                model.FechaFin,
                model.EsPublico,
                model.Presupuesto,
                model.TipoViaje,
                model.Estado,
                p_autor_oid,
                model.CarpetasId.GetValueOrDefault()
            );

            if (model.CarpetasId is int referenceCarpetas)
            {
                entity.Carpetas = await unitOfWork.CarpetaRepository.ReadAsync(referenceCarpetas)
                    ?? throw new InvalidOperationException("El registro seleccionado de Carpeta ya no existe.");
            }
            if (model.CollagesId is int referenceCollages)
            {
                entity.Collages = await unitOfWork.CollageRepository.ReadAsync(referenceCollages)
                    ?? throw new InvalidOperationException("El registro seleccionado de Collage ya no existe.");
            }

            await service.ModifierViajeDefaultAsync(entity);
            await unitOfWork.CommitAsync();
        }
        catch (Exception error)
        {
            await unitOfWork.RollbackAsync();
            if (error is OperationCanceledException) throw;
            _logger.LogError(error, "Could not save the record and its lines.");
            ModelState.AddModelError(string.Empty, "No se han podido guardar el registro y sus líneas.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ViajeController_Delete_GET_body) START*/
        var entity = await _viajeService.LeerViajeOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ViajeAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ViajeController_DeleteConfirmed_POST_body) START*/
        if (!ModelState.IsValid) return View("Delete", new ViajeViewModel());
        await _viajeService.BorrarViajeAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_ViajesPublicos_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ViajesPublicos()
    {
    /*PROTECTED REGION ID(ViajeController_ViajesPublicos_GET_body) START*/
        var result = await _viajeService.ViajesPublicosAsync();
        return Ok(result.Select(ViajeAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ViajeController_ViajesPorEstado_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ViajesPorEstado(EstadoViaje p_estado)
    {
    /*PROTECTED REGION ID(ViajeController_ViajesPorEstado_GET_body) START*/
        var result = await _viajeService.ViajesPorEstadoAsync(p_estado);
        return Ok(result.Select(ViajeAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_AgregarParada_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarParada(int p_oid, IReadOnlyList<int> p_paradas_oid)
    {
    /*PROTECTED REGION ID(ViajeController_AgregarParada_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.AgregarParadaAsync(p_oid, p_paradas_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_AgregarParadaDestino_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarParadaDestino(int p_oid, IReadOnlyList<int> p_paradasDestino_oid)
    {
    /*PROTECTED REGION ID(ViajeController_AgregarParadaDestino_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.AgregarParadaDestinoAsync(p_oid, p_paradasDestino_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_CompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(ViajeController_CompartirConCliente_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.CompartirConClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_DejarDeCompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DejarDeCompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(ViajeController_DejarDeCompartirConCliente_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.DejarDeCompartirConClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_AgregarResena_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarResena(int p_oid, IReadOnlyList<int> p_resenas_oid)
    {
    /*PROTECTED REGION ID(ViajeController_AgregarResena_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.AgregarResenaAsync(p_oid, p_resenas_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_AsignarCarpeta_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarCarpeta(int p_oid, int p_carpeta_oid)
    {
    /*PROTECTED REGION ID(ViajeController_AsignarCarpeta_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.AsignarCarpetaAsync(p_oid, p_carpeta_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_AsignarCollage_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarCollage(int p_oid, int p_collage_oid)
    {
    /*PROTECTED REGION ID(ViajeController_AsignarCollage_POST_body) START*/
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _viajeService.AsignarCollageAsync(p_oid, p_collage_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(ViajeController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsViajeViewModel) START*/
    private async Task LoadFormOptionsAsync(ViajeViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsViajeViewModel_DSMOdyssApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(ViajeViewModel model, DSM_Odyss.Application.Abstractions.IUnitOfWork unitOfWork)
    {
        var options0 = (await unitOfWork.CarpetaRepository.ReadAllAsync())
            .Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = item.Id.ToString(), Text = item.Nombre ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.CollageRepository.ReadAllAsync())
            .Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = item.Id.ToString(), Text = item.UrlImagen ?? string.Empty }).ToList();
        var optionsDestinos = (await unitOfWork.DestinoRepository.ReadAllAsync())
            .Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = item.Id.ToString(), Text = $"{item.Ciudad} ({item.Pais})" }).ToList();

        model.CarpetasOptions = options0;
        model.CollagesOptions = options1;
        model.DestinosCatalogoOptions = optionsDestinos;
    }
    /*PROTECTED REGION END*/
}