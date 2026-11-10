// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(RecordatorioController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(RecordatorioController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class RecordatorioController : Controller
{
    /*PROTECTED REGION ID(RecordatorioController_dependencies) START*/
    private readonly RecordatorioService _recordatorioService;
    private readonly ILogger<RecordatorioController> _logger;
    private readonly DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory _formUnitOfWorkFactory;
    public RecordatorioController(RecordatorioService recordatorioService, DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory formUnitOfWorkFactory, ILogger<RecordatorioController>? logger = null)
    {
        _recordatorioService = recordatorioService;

        _formUnitOfWorkFactory = formUnitOfWorkFactory;

        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<RecordatorioController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(RecordatorioController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(RecordatorioController_Index_GET_body) START*/

        var entities = await _recordatorioService.LeerTodosRecordatoriosAsync();
        return View(entities.Select(RecordatorioAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(RecordatorioController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(RecordatorioController_Details_GET_body) START*/

        var entity = await _recordatorioService.LeerRecordatorioOIDAsync(id);
        if (entity is null) return NotFound();
        return View(RecordatorioAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(RecordatorioController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create()
    {
    /*PROTECTED REGION ID(RecordatorioController_Create_GET_body) START*/

        var model = new DSM_Odyss.Web.ViewModels.RecordatorioViewModel();

        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.RecordatorioViewModel model)
    {
    /*PROTECTED REGION ID(RecordatorioController_Create_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new RecordatorioService(unitOfWork);



        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.TareaId is int selectedTarea &&
            !model.TareaOptions.Any(option => option.Value == Convert.ToString(selectedTarea, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("TareaId", "Selecciona un registro existente de Tarea.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Recordatorio.Create");
        try
        {

            var entity = await service.CrearRecordatorioAsync(model.Titulo, model.Detalles, model.Fecha, model.Hora, model.TipoAviso, model.Enviado);
            entity.Tarea = model.TareaId is int referenceTarea
                ? await unitOfWork.TareaRepository.ReadAsync(referenceTarea) ?? throw new InvalidOperationException("El registro seleccionado de Tarea ya no existe.") : null!;

            await service.ModifierRecordatorioDefaultAsync(entity);

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
    /*PROTECTED REGION ID(RecordatorioController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(RecordatorioController_Edit_GET_body) START*/

        var entity = await _recordatorioService.LeerRecordatorioOIDAsync(id);
        if (entity is null) return NotFound();
        var model = RecordatorioAssembler.ToViewModel(entity);
        await LoadFormOptionsAsync(model);
        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.RecordatorioViewModel model)
    {
    /*PROTECTED REGION ID(RecordatorioController_Edit_POST_body) START*/


        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var service = new RecordatorioService(unitOfWork);
        var entity = await service.ReadOIDRecordatorioDefaultAsync(id);
        if (entity is null) return NotFound();


        await LoadFormOptionsAsync(model, unitOfWork);
        if (model.TareaId is int selectedTarea &&
            !model.TareaOptions.Any(option => option.Value == Convert.ToString(selectedTarea, System.Globalization.CultureInfo.InvariantCulture)))
            ModelState.AddModelError("TareaId", "Selecciona un registro existente de Tarea.");



        if (!ModelState.IsValid) return View(model);

        await unitOfWork.BeginTransactionAsync("MVC.Recordatorio.Edit");
        try
        {

            await service.ModificarRecordatorioAsync(id, model.Titulo, model.Detalles, model.Fecha, model.Hora, model.TipoAviso, model.Enviado);
            entity.Tarea = model.TareaId is int referenceTarea
                ? await unitOfWork.TareaRepository.ReadAsync(referenceTarea) ?? throw new InvalidOperationException("El registro seleccionado de Tarea ya no existe.") : null!;

            await service.ModifierRecordatorioDefaultAsync(entity);

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
    /*PROTECTED REGION ID(RecordatorioController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(RecordatorioController_Delete_GET_body) START*/

        var entity = await _recordatorioService.LeerRecordatorioOIDAsync(id);
        if (entity is null) return NotFound();
        return View(RecordatorioAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(RecordatorioController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.RecordatorioViewModel());
        await _recordatorioService.BorrarRecordatorioAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(RecordatorioController_Pendientes_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Pendientes()
    {
    /*PROTECTED REGION ID(RecordatorioController_Pendientes_GET_body) START*/

        var result = await _recordatorioService.PendientesAsync();
        return Ok(result.Select(RecordatorioAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_AsignarTarea_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarTarea(int p_oid, int p_tarea_oid)
    {
    /*PROTECTED REGION ID(RecordatorioController_AsignarTarea_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _recordatorioService.AsignarTareaAsync(p_oid, p_tarea_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_DesasignarTarea_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DesasignarTarea(int p_oid, int p_tarea_oid)
    {
    /*PROTECTED REGION ID(RecordatorioController_DesasignarTarea_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _recordatorioService.DesasignarTareaAsync(p_oid, p_tarea_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(RecordatorioController_AsignarCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(RecordatorioController_AsignarCliente_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _recordatorioService.AsignarClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    /*PROTECTED REGION ID(RecordatorioController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsRecordatorioViewModel) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.RecordatorioViewModel model)
    {
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        await LoadFormOptionsAsync(model, unitOfWork);
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RecordatorioController_method_LoadFormOptionsAsync_DSMOdyssWebViewModelsRecordatorioViewModel_DSMOdyssApplicationAbstractionsIUnitOfWork) START*/
    private async Task LoadFormOptionsAsync(DSM_Odyss.Web.ViewModels.RecordatorioViewModel model, DSM_Odyss.Application.Abstractions.IUnitOfWork unitOfWork)
    {

        var options0 = (await unitOfWork.TareaRepository.ReadAllAsync()).Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = Convert.ToString(item.Id, System.Globalization.CultureInfo.InvariantCulture), Text = Convert.ToString(item.Descripcion, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty }).ToList();
        model.TareaOptions = options0.Select(item => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = item.Value,
            Text = item.Text
        }).ToList();

    }
    /*PROTECTED REGION END*/

}
