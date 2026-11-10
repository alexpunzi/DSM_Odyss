// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(CollageController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(CollageController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class CollageController : Controller
{
    /*PROTECTED REGION ID(CollageController_dependencies) START*/
    private readonly CollageService _collageService;
    private readonly ILogger<CollageController> _logger;
    public CollageController(CollageService collageService, ILogger<CollageController>? logger = null)
    {
        _collageService = collageService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<CollageController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(CollageController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(CollageController_Index_GET_body) START*/

        var entities = await _collageService.LeerTodasCollagesAsync();
        return View(entities.Select(CollageAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CollageController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(CollageController_Details_GET_body) START*/

        var entity = await _collageService.LeerCollageOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CollageAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CollageController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(CollageController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.CollageViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CollageController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.CollageViewModel model, int p_viaje_oid)
    {
    /*PROTECTED REGION ID(CollageController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _collageService.CrearCollageAsync(model.UrlImagen, model.FechaCreacion, p_viaje_oid);
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
    /*PROTECTED REGION ID(CollageController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(CollageController_Edit_GET_body) START*/

        var entity = await _collageService.LeerCollageOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CollageAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CollageController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.CollageViewModel model, int p_viaje_oid)
    {
    /*PROTECTED REGION ID(CollageController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _collageService.ModificarCollageAsync(id, model.UrlImagen, model.FechaCreacion, p_viaje_oid);
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
    /*PROTECTED REGION ID(CollageController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(CollageController_Delete_GET_body) START*/

        var entity = await _collageService.LeerCollageOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CollageAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CollageController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(CollageController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.CollageViewModel());
        await _collageService.BorrarCollageAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CollageController_DelAnyo_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DelAnyo(int p_anyo)
    {
    /*PROTECTED REGION ID(CollageController_DelAnyo_GET_body) START*/

        var result = await _collageService.DelAnyoAsync(p_anyo);
        return Ok(result.Select(CollageAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
