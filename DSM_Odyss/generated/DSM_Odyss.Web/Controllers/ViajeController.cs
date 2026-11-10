// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ViajeController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

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

        var model = new DSM_Odyss.Web.ViewModels.ViajeViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ViajeController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ViajeViewModel model, int p_autor_oid)
    {
    /*PROTECTED REGION ID(ViajeController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ViajeService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.CarpetasId is int selectedCarpetas &&
            !model.CarpetasOptions.Any(option => option.Value == Convert.ToString(selectedCarpetas, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CarpetasId", "Selecciona un registro existente de Carpeta.");

        if (model.CollagesId is int selectedCollages &&
            !model.CollagesOptions.Any(option => option.Value == Convert.ToString(selectedCollages, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CollagesId", "Selecciona un registro existente de Collage.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Create");
        try
        {

            var entity = await service.CrearViajeAsync(model.Titulo, model.FechaInicio, model.FechaFin, model.EsPublico, model.Presupuesto, model.TipoViaje, model.Estado, p_autor_oid, model.CarpetasId.GetValueOrDefault());
            entity.Carpetas = model.CarpetasId is int referenceCarpetas
                ? await unitOfWork.CarpetaRepository.ReadAsync(referenceCarpetas) ?? throw new InvalidOperationException("El registro seleccionado de Carpeta ya no existe.") : null!;
            entity.Collages = model.CollagesId is int referenceCollages
                ? await unitOfWork.CollageRepository.ReadAsync(referenceCollages) ?? throw new InvalidOperationException("El registro seleccionado de Collage ya no existe.") : null!;

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
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ViajeViewModel model, int p_autor_oid)
    {
    /*PROTECTED REGION ID(ViajeController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ViajeService(unitOfWork);
        var entity = await service.ReadOIDViajeDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.CarpetasId is int selectedCarpetas &&
            !model.CarpetasOptions.Any(option => option.Value == Convert.ToString(selectedCarpetas, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CarpetasId", "Selecciona un registro existente de Carpeta.");

        if (model.CollagesId is int selectedCollages &&
            !model.CollagesOptions.Any(option => option.Value == Convert.ToString(selectedCollages, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("CollagesId", "Selecciona un registro existente de Collage.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Viaje.Edit");
        try
        {

            await service.ModificarViajeAsync(id, model.Titulo, model.FechaInicio, model.FechaFin, model.EsPublico, model.Presupuesto, model.TipoViaje, model.Estado, p_autor_oid, model.CarpetasId.GetValueOrDefault());
            entity.Carpetas = model.CarpetasId is int referenceCarpetas
                ? await unitOfWork.CarpetaRepository.ReadAsync(referenceCarpetas) ?? throw new InvalidOperationException("El registro seleccionado de Carpeta ya no existe.") : null!;
            entity.Collages = model.CollagesId is int referenceCollages
                ? await unitOfWork.CollageRepository.ReadAsync(referenceCollages) ?? throw new InvalidOperationException("El registro seleccionado de Collage ya no existe.") : null!;

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

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ViajeViewModel());
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
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ViajeViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsViajeViewModel_DSMOdyssApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ViajeViewModel model, DSM_Odyss.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.CarpetaRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Nombre, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.CollageRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.UrlImagen, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.CarpetasOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.CollagesOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
