// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ChecklistController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ChecklistController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ChecklistController : Controller
{
    /*PROTECTED REGION ID(ChecklistController_dependencies) START*/
    private readonly ChecklistService _checklistService;
    private readonly ILogger<ChecklistController> _logger;
    public ChecklistController(ChecklistService checklistService, ILogger<ChecklistController>? logger = null)
    {
        _checklistService = checklistService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ChecklistController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ChecklistController_Index_GET_body) START*/

        var entities = await _checklistService.LeerTodosChecklistsAsync();
        return View(entities.Select(ChecklistAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_Details_GET_body) START*/

        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ChecklistAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(ChecklistController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.ChecklistViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ChecklistViewModel model)
    {
    /*PROTECTED REGION ID(ChecklistController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _checklistService.CrearChecklistAsync(model.Nombre);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not create the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_Edit_GET_body) START*/

        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ChecklistAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ChecklistViewModel model)
    {
    /*PROTECTED REGION ID(ChecklistController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _checklistService.ModificarChecklistAsync(id, model.Nombre);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not update the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_Delete_GET_body) START*/

        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ChecklistAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ChecklistViewModel());
        await _checklistService.BorrarChecklistAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_ConTareas_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConTareas()
    {
    /*PROTECTED REGION ID(ChecklistController_ConTareas_GET_body) START*/

        var result = await _checklistService.ConTareasAsync();
        return Ok(result.Select(ChecklistAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_AgregarTarea_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarTarea(int p_oid, IReadOnlyList<int> p_tareas_oid)
    {
    /*PROTECTED REGION ID(ChecklistController_AgregarTarea_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _checklistService.AgregarTareaAsync(p_oid, p_tareas_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_AsignarCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AsignarCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(ChecklistController_AsignarCliente_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _checklistService.AsignarClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
