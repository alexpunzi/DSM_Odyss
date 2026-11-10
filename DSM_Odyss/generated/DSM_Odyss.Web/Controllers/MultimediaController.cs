// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(MultimediaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(MultimediaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class MultimediaController : Controller
{
    /*PROTECTED REGION ID(MultimediaController_dependencies) START*/
    private readonly MultimediaService _multimediaService;
    private readonly ILogger<MultimediaController> _logger;
    public MultimediaController(MultimediaService multimediaService, ILogger<MultimediaController>? logger = null)
    {
        _multimediaService = multimediaService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<MultimediaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(MultimediaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(MultimediaController_Index_GET_body) START*/

        var entities = await _multimediaService.LeerTodasMultimdiaAsync();
        return View(entities.Select(MultimediaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(MultimediaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(MultimediaController_Details_GET_body) START*/

        var entity = await _multimediaService.LeerMultimediaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(MultimediaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(MultimediaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(MultimediaController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.MultimediaViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(MultimediaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.MultimediaViewModel model, int p_carpeta_oid)
    {
    /*PROTECTED REGION ID(MultimediaController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _multimediaService.CrearMultimediaAsync(model.Tipo, model.Nombre, model.FechaSubida, model.Descripcion, p_carpeta_oid);
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
    /*PROTECTED REGION ID(MultimediaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(MultimediaController_Edit_GET_body) START*/

        var entity = await _multimediaService.LeerMultimediaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(MultimediaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(MultimediaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.MultimediaViewModel model, int p_carpeta_oid)
    {
    /*PROTECTED REGION ID(MultimediaController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _multimediaService.ModificarMultimediaAsync(id, model.Tipo, model.Nombre, model.FechaSubida, model.Descripcion, p_carpeta_oid);
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
    /*PROTECTED REGION ID(MultimediaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(MultimediaController_Delete_GET_body) START*/

        var entity = await _multimediaService.LeerMultimediaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(MultimediaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(MultimediaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(MultimediaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.MultimediaViewModel());
        await _multimediaService.BorrarMultimediaAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(MultimediaController_DelTipo_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DelTipo(TipoMultimedia p_tipo)
    {
    /*PROTECTED REGION ID(MultimediaController_DelTipo_GET_body) START*/

        var result = await _multimediaService.DelTipoAsync(p_tipo);
        return Ok(result.Select(MultimediaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
