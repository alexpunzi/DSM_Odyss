// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(DestinoController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(DestinoController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class DestinoController : Controller
{
    /*PROTECTED REGION ID(DestinoController_dependencies) START*/
    private readonly DestinoService _destinoService;
    private readonly ILogger<DestinoController> _logger;
    public DestinoController(DestinoService destinoService, ILogger<DestinoController>? logger = null)
    {
        _destinoService = destinoService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<DestinoController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(DestinoController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(DestinoController_Index_GET_body) START*/

        var entities = await _destinoService.LeerTodasDestinosAsync();
        return View(entities.Select(DestinoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(DestinoController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(DestinoController_Details_GET_body) START*/

        var entity = await _destinoService.LeerDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        return View(DestinoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(DestinoController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(DestinoController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.DestinoViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DestinoController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.DestinoViewModel model)
    {
    /*PROTECTED REGION ID(DestinoController_Create_POST_body) START*/

        ModelState.Remove(nameof(model.Ciudad)); // Campo fuera del contrato Create de Application.

        if (!ModelState.IsValid) return View(model);

        try
        {
            await _destinoService.CrearDestinoAsync(model.Pais);
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
    /*PROTECTED REGION ID(DestinoController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(DestinoController_Edit_GET_body) START*/

        var entity = await _destinoService.LeerDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        return View(DestinoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DestinoController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.DestinoViewModel model)
    {
    /*PROTECTED REGION ID(DestinoController_Edit_POST_body) START*/

        ModelState.Remove(nameof(model.Ciudad)); // Campo fuera del contrato Edit de Application.
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _destinoService.ModificarDestinoAsync(id, model.Pais);
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
    /*PROTECTED REGION ID(DestinoController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(DestinoController_Delete_GET_body) START*/

        var entity = await _destinoService.LeerDestinoOIDAsync(id);
        if (entity is null) return NotFound();
        return View(DestinoAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(DestinoController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(DestinoController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.DestinoViewModel());
        await _destinoService.BorrarDestinoAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(DestinoController_PorPais_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PorPais(string p_pais)
    {
    /*PROTECTED REGION ID(DestinoController_PorPais_GET_body) START*/

        var result = await _destinoService.PorPaisAsync(p_pais);
        return Ok(result.Select(DestinoAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
