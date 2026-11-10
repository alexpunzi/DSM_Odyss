// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(TareaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(TareaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class TareaController : Controller
{
    /*PROTECTED REGION ID(TareaController_dependencies) START*/
    private readonly TareaService _tareaService;
    private readonly ILogger<TareaController> _logger;
    public TareaController(TareaService tareaService, ILogger<TareaController>? logger = null)
    {
        _tareaService = tareaService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<TareaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(TareaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(TareaController_Index_GET_body) START*/

        var entities = await _tareaService.LeerTodasTareasAsync();
        return View(entities.Select(TareaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TareaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(TareaController_Details_GET_body) START*/

        var entity = await _tareaService.LeerTareaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(TareaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TareaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(TareaController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.TareaViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TareaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.TareaViewModel model, int p_checklist_oid)
    {
    /*PROTECTED REGION ID(TareaController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _tareaService.CrearTareaAsync(model.Descripcion, model.Completada, p_checklist_oid);
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
    /*PROTECTED REGION ID(TareaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(TareaController_Edit_GET_body) START*/

        var entity = await _tareaService.LeerTareaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(TareaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TareaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.TareaViewModel model, int p_checklist_oid)
    {
    /*PROTECTED REGION ID(TareaController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _tareaService.ModificarTareaAsync(id, model.Descripcion, model.Completada, p_checklist_oid);
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
    /*PROTECTED REGION ID(TareaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(TareaController_Delete_GET_body) START*/

        var entity = await _tareaService.LeerTareaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(TareaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(TareaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(TareaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.TareaViewModel());
        await _tareaService.BorrarTareaAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(TareaController_PorEstado_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PorEstado(bool p_completada)
    {
    /*PROTECTED REGION ID(TareaController_PorEstado_GET_body) START*/

        var result = await _tareaService.PorEstadoAsync(p_completada);
        return Ok(result.Select(TareaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
