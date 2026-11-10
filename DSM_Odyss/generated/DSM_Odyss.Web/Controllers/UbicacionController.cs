// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(UbicacionController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(UbicacionController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class UbicacionController : Controller
{
    /*PROTECTED REGION ID(UbicacionController_dependencies) START*/
    private readonly UbicacionService _ubicacionService;
    private readonly ILogger<UbicacionController> _logger;
    public UbicacionController(UbicacionService ubicacionService, ILogger<UbicacionController>? logger = null)
    {
        _ubicacionService = ubicacionService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<UbicacionController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(UbicacionController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(UbicacionController_Index_GET_body) START*/

        var entities = await _ubicacionService.LeerTodasUbicacionesAsync();
        return View(entities.Select(UbicacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UbicacionController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(UbicacionController_Details_GET_body) START*/

        var entity = await _ubicacionService.LeerUbicacionOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UbicacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UbicacionController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(UbicacionController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.UbicacionViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UbicacionController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.UbicacionViewModel model)
    {
    /*PROTECTED REGION ID(UbicacionController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _ubicacionService.CrearUbicacionAsync(model.Nombre, model.Latitud, model.Longitud);
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
    /*PROTECTED REGION ID(UbicacionController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(UbicacionController_Edit_GET_body) START*/

        var entity = await _ubicacionService.LeerUbicacionOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UbicacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UbicacionController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.UbicacionViewModel model)
    {
    /*PROTECTED REGION ID(UbicacionController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _ubicacionService.ModificarUbicacionAsync(id, model.Nombre, model.Latitud, model.Longitud);
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
    /*PROTECTED REGION ID(UbicacionController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(UbicacionController_Delete_GET_body) START*/

        var entity = await _ubicacionService.LeerUbicacionOIDAsync(id);
        if (entity is null) return NotFound();
        return View(UbicacionAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(UbicacionController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(UbicacionController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.UbicacionViewModel());
        await _ubicacionService.BorrarUbicacionAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(UbicacionController_ConNombre_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConNombre(string p_nombre)
    {
    /*PROTECTED REGION ID(UbicacionController_ConNombre_GET_body) START*/

        var result = await _ubicacionService.ConNombreAsync(p_nombre);
        return Ok(result.Select(UbicacionAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
