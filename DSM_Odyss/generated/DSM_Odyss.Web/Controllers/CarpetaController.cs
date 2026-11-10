// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(CarpetaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(CarpetaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class CarpetaController : Controller
{
    /*PROTECTED REGION ID(CarpetaController_dependencies) START*/
    private readonly CarpetaService _carpetaService;
    private readonly ILogger<CarpetaController> _logger;
    public CarpetaController(CarpetaService carpetaService, ILogger<CarpetaController>? logger = null)
    {
        _carpetaService = carpetaService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<CarpetaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(CarpetaController_Index_GET_body) START*/

        var entities = await _carpetaService.LeerTodasCarpetasAsync();
        return View(entities.Select(CarpetaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_Details_GET_body) START*/

        var entity = await _carpetaService.LeerCarpetaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CarpetaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(CarpetaController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.CarpetaViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.CarpetaViewModel model, int p_viaje_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _carpetaService.CrearCarpetaAsync(model.Nombre, p_viaje_oid);
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
    /*PROTECTED REGION ID(CarpetaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_Edit_GET_body) START*/

        var entity = await _carpetaService.LeerCarpetaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CarpetaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.CarpetaViewModel model, int p_viaje_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _carpetaService.ModificarCarpetaAsync(id, model.Nombre, p_viaje_oid);
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
    /*PROTECTED REGION ID(CarpetaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_Delete_GET_body) START*/

        var entity = await _carpetaService.LeerCarpetaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(CarpetaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(CarpetaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.CarpetaViewModel());
        await _carpetaService.BorrarCarpetaAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(CarpetaController_ConMultimedia_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConMultimedia()
    {
    /*PROTECTED REGION ID(CarpetaController_ConMultimedia_GET_body) START*/

        var result = await _carpetaService.ConMultimediaAsync();
        return Ok(result.Select(CarpetaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_AgregarMultimedia_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarMultimedia(int p_oid, IReadOnlyList<int> p_multimedia_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_AgregarMultimedia_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _carpetaService.AgregarMultimediaAsync(p_oid, p_multimedia_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_CompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> CompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_CompartirConCliente_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _carpetaService.CompartirConClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(CarpetaController_DejarDeCompartirConCliente_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DejarDeCompartirConCliente(int p_oid, IReadOnlyList<int> p_clientes_oid)
    {
    /*PROTECTED REGION ID(CarpetaController_DejarDeCompartirConCliente_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _carpetaService.DejarDeCompartirConClienteAsync(p_oid, p_clientes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
