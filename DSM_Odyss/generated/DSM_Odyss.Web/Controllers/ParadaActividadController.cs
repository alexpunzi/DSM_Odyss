// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ParadaActividadController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ParadaActividadController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ParadaActividadController : Controller
{
    /*PROTECTED REGION ID(ParadaActividadController_dependencies) START*/
    private readonly ParadaActividadService _paradaActividadService;
    private readonly ILogger<ParadaActividadController> _logger;
    private readonly DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public ParadaActividadController(ParadaActividadService paradaActividadService, DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<ParadaActividadController>? logger = null)
    {
        _paradaActividadService = paradaActividadService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;

        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ParadaActividadController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ParadaActividadController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ParadaActividadController_Index_GET_body) START*/

        var entities = await _paradaActividadService.LeerTodasParadasActividadesAsync();
        return View(entities.Select(ParadaActividadAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaActividadController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ParadaActividadController_Details_GET_body) START*/

        var entity = await _paradaActividadService.LeerParadaActividadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ParadaActividadAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaActividadController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(ParadaActividadController_Create_GET_body) START*/

        var model = new DSM_Odyss.Web.ViewModels.ParadaActividadViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ParadaActividadViewModel model)
    {
    /*PROTECTED REGION ID(ParadaActividadController_Create_POST_body) START*/

        ModelState.Remove(nameof(model.Detalles)); // Campo fuera del contrato Create de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ParadaActividadService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.ViajeId is int selectedViaje &&
            !model.ViajeOptions.Any(option => option.Value == Convert.ToString(selectedViaje, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ViajeId", "Selecciona un registro existente de Viaje.");
        if (model.ViajeId is null) ModelState.AddModelError("ViajeId", "Debes seleccionar una opción.");
        if (model.UbicacionId is int selectedUbicacion &&
            !model.UbicacionOptions.Any(option => option.Value == Convert.ToString(selectedUbicacion, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UbicacionId", "Selecciona un registro existente de Ubicacion.");

        if (model.ParadaDestinoId is int selectedParadaDestino &&
            !model.ParadaDestinoOptions.Any(option => option.Value == Convert.ToString(selectedParadaDestino, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ParadaDestinoId", "Selecciona un registro existente de ParadaDestino.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.ParadaActividad.Create");
        try
        {

            var entity = await service.CrearParadaActividadAsync(model.Nombre, model.Dia, model.Horario, model.Completada, model.ViajeId.GetValueOrDefault());
            entity.Viaje = model.ViajeId is int referenceViaje
                ? await unitOfWork.ViajeRepository.ReadAsync(referenceViaje) ?? throw new InvalidOperationException("El registro seleccionado de Viaje ya no existe.") : null!;
            entity.Ubicacion = model.UbicacionId is int referenceUbicacion
                ? await unitOfWork.UbicacionRepository.ReadAsync(referenceUbicacion) ?? throw new InvalidOperationException("El registro seleccionado de Ubicacion ya no existe.") : null!;
            entity.ParadaDestino = model.ParadaDestinoId is int referenceParadaDestino
                ? await unitOfWork.ParadaDestinoRepository.ReadAsync(referenceParadaDestino) ?? throw new InvalidOperationException("El registro seleccionado de ParadaDestino ya no existe.") : null!;

            await service.ModifierParadaActividadDefaultAsync(entity);

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
    /*PROTECTED REGION ID(ParadaActividadController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ParadaActividadController_Edit_GET_body) START*/

        var entity = await _paradaActividadService.LeerParadaActividadOIDAsync(id);
        if (entity is null) return NotFound();
        var model = ParadaActividadAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ParadaActividadViewModel model)
    {
    /*PROTECTED REGION ID(ParadaActividadController_Edit_POST_body) START*/

        ModelState.Remove(nameof(model.Detalles)); // Campo fuera del contrato Edit de Application.
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new ParadaActividadService(unitOfWork);
        var entity = await service.ReadOIDParadaActividadDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.ViajeId is int selectedViaje &&
            !model.ViajeOptions.Any(option => option.Value == Convert.ToString(selectedViaje, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ViajeId", "Selecciona un registro existente de Viaje.");
        if (model.ViajeId is null) ModelState.AddModelError("ViajeId", "Debes seleccionar una opción.");
        if (model.UbicacionId is int selectedUbicacion &&
            !model.UbicacionOptions.Any(option => option.Value == Convert.ToString(selectedUbicacion, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("UbicacionId", "Selecciona un registro existente de Ubicacion.");

        if (model.ParadaDestinoId is int selectedParadaDestino &&
            !model.ParadaDestinoOptions.Any(option => option.Value == Convert.ToString(selectedParadaDestino, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("ParadaDestinoId", "Selecciona un registro existente de ParadaDestino.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.ParadaActividad.Edit");
        try
        {

            await service.ModificarParadaActividadAsync(id, model.Nombre, model.Dia, model.Horario, model.Completada, model.ViajeId.GetValueOrDefault());
            entity.Viaje = model.ViajeId is int referenceViaje
                ? await unitOfWork.ViajeRepository.ReadAsync(referenceViaje) ?? throw new InvalidOperationException("El registro seleccionado de Viaje ya no existe.") : null!;
            entity.Ubicacion = model.UbicacionId is int referenceUbicacion
                ? await unitOfWork.UbicacionRepository.ReadAsync(referenceUbicacion) ?? throw new InvalidOperationException("El registro seleccionado de Ubicacion ya no existe.") : null!;
            entity.ParadaDestino = model.ParadaDestinoId is int referenceParadaDestino
                ? await unitOfWork.ParadaDestinoRepository.ReadAsync(referenceParadaDestino) ?? throw new InvalidOperationException("El registro seleccionado de ParadaDestino ya no existe.") : null!;

            await service.ModifierParadaActividadDefaultAsync(entity);

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
    /*PROTECTED REGION ID(ParadaActividadController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ParadaActividadController_Delete_GET_body) START*/

        var entity = await _paradaActividadService.LeerParadaActividadOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ParadaActividadAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ParadaActividadController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ParadaActividadViewModel());
        await _paradaActividadService.BorrarParadaActividadAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ParadaActividadController_Pendientes_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Pendientes()
    {
    /*PROTECTED REGION ID(ParadaActividadController_Pendientes_GET_body) START*/

        var result = await _paradaActividadService.PendientesAsync();
        return Ok(result.Select(ParadaActividadAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_AsignarUbicacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarUbicacion(int p_oid, int p_ubicacion_oid)
    {
    /*PROTECTED REGION ID(ParadaActividadController_AsignarUbicacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.AsignarUbicacionAsync(p_oid, p_ubicacion_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_DesasignarUbicacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DesasignarUbicacion(int p_oid, int p_ubicacion_oid)
    {
    /*PROTECTED REGION ID(ParadaActividadController_DesasignarUbicacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.DesasignarUbicacionAsync(p_oid, p_ubicacion_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_AsignarParadaDestino_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarParadaDestino(int p_oid, int p_paradaDestino_oid)
    {
    /*PROTECTED REGION ID(ParadaActividadController_AsignarParadaDestino_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.AsignarParadaDestinoAsync(p_oid, p_paradaDestino_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ParadaActividadController_DesasignarParadaDestino_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DesasignarParadaDestino(int p_oid, int p_paradaDestino_oid)
    {
    /*PROTECTED REGION ID(ParadaActividadController_DesasignarParadaDestino_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _paradaActividadService.DesasignarParadaDestinoAsync(p_oid, p_paradaDestino_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(ParadaActividadController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsParadaActividadViewModel) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ParadaActividadViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaActividadController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsParadaActividadViewModel_DSMOdyssApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.ParadaActividadViewModel model, DSM_Odyss.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.ViajeRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Titulo, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options1 = (await unitOfWork.UbicacionRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Nombre, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        var options2 = (await unitOfWork.ParadaDestinoRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.ViajeOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.UbicacionOptions = options1.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();
        model.ParadaDestinoOptions = options2.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
