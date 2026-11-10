// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ParadaDestinoController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ParadaDestinoController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ParadaDestinoController : Controller
{
    /*PROTECTED REGION ID(ParadaDestinoController_dependencies) START*/
    private readonly ParadaDestinoService _paradaDestinoService;
    private readonly ILogger<ParadaDestinoController> _logger;
    private readonly DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public ParadaDestinoController(ParadaDestinoService paradaDestinoService, DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<ParadaDestinoController>? logger = null)
    {
        _paradaDestinoService = paradaDestinoService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;

        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ParadaDestinoController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ParadaDestinoController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Index_GET_body) START*/

        var entities = await _paradaDestinoService.LeerTodasParadasDestinoAsync();
        return View(entities.Select(ParadaDestinoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaDestinoController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Details_GET_body) START*/

        var entity = await _paradaDestinoService.LeerParadaDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ParadaDestinoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaDestinoController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Create_GET_body) START*/

        var model = new DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaDestinoController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel model)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ParadaDestinoService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.ViajeId is int selectedViaje &&
            !model.ViajeOptions.Any(option => option.Value == Convert.ToString(selectedViaje, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ViajeId", "Selecciona un registro existente de Viaje.");
        if (model.ViajeId is null) ModelState.AddModelError("ViajeId", "Debes seleccionar una opción.");
        if (model.DestinoId is int selectedDestino &&
            !model.DestinoOptions.Any(option => option.Value == Convert.ToString(selectedDestino, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DestinoId", "Selecciona un registro existente de Destino.");
        if (model.DestinoId is null) ModelState.AddModelError("DestinoId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.ParadaDestino.Create");
        try
        {

            var entity = await service.CrearParadaDestinoAsync(model.Orden, model.FechaLlegada, model.FechaSalida, model.ViajeId.GetValueOrDefault(), model.DestinoId.GetValueOrDefault());
            entity.Viaje = model.ViajeId is int referenceViaje
                ? await unitOfWork.ViajeRepository.ReadAsync(referenceViaje) ?? throw new InvalidOperationException("El registro seleccionado de Viaje ya no existe.") : null!;
            entity.Destino = model.DestinoId is int referenceDestino
                ? await unitOfWork.DestinoRepository.ReadAsync(referenceDestino) ?? throw new InvalidOperationException("El registro seleccionado de Destino ya no existe.") : null!;

            await service.ModifierParadaDestinoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(ParadaDestinoController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Edit_GET_body) START*/

        var entity = await _paradaDestinoService.LeerParadaDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        var model = ParadaDestinoAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaDestinoController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel model)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ParadaDestinoService(unitOfWork);
        var entity = await service.ReadOIDParadaDestinoDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.ViajeId is int selectedViaje &&
            !model.ViajeOptions.Any(option => option.Value == Convert.ToString(selectedViaje, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ViajeId", "Selecciona un registro existente de Viaje.");
        if (model.ViajeId is null) ModelState.AddModelError("ViajeId", "Debes seleccionar una opción.");
        if (model.DestinoId is int selectedDestino &&
            !model.DestinoOptions.Any(option => option.Value == Convert.ToString(selectedDestino, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("DestinoId", "Selecciona un registro existente de Destino.");
        if (model.DestinoId is null) ModelState.AddModelError("DestinoId", "Debes seleccionar una opción.");


        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.ParadaDestino.Edit");
        try
        {

            await service.ModificarParadaDestinoAsync(id, model.Orden, model.FechaLlegada, model.FechaSalida, model.ViajeId.GetValueOrDefault(), model.DestinoId.GetValueOrDefault());
            entity.Viaje = model.ViajeId is int referenceViaje
                ? await unitOfWork.ViajeRepository.ReadAsync(referenceViaje) ?? throw new InvalidOperationException("El registro seleccionado de Viaje ya no existe.") : null!;
            entity.Destino = model.DestinoId is int referenceDestino
                ? await unitOfWork.DestinoRepository.ReadAsync(referenceDestino) ?? throw new InvalidOperationException("El registro seleccionado de Destino ya no existe.") : null!;

            await service.ModifierParadaDestinoDefaultAsync(entity);

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
    /*PROTECTED REGION ID(ParadaDestinoController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_Delete_GET_body) START*/

        var entity = await _paradaDestinoService.LeerParadaDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ParadaDestinoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaDestinoController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel());
        await _paradaDestinoService.BorrarParadaDestinoAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaDestinoController_PorViaje_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PorViaje(int p_viaje_oid)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_PorViaje_GET_body) START*/

        var result = await _paradaDestinoService.PorViajeAsync(p_viaje_oid);
        return Ok(result.Select(ParadaDestinoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaDestinoController_AsignarActividades_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarActividades(int p_oid, IReadOnlyList<int> p_actividades_oid)
    {
    /*PROTECTED REGION ID(ParadaDestinoController_AsignarActividades_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaDestinoService.AsignarActividadesAsync(p_oid, p_actividades_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(ParadaDestinoController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsParadaDestinoViewModel) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaDestinoController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsParadaDestinoViewModel_DSMOdyssApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ParadaDestinoViewModel model, DSM_Odyss.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.ViajeRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.DestinoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Pais, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.ViajeOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.DestinoOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
