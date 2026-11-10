// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(EstadisticasController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(EstadisticasController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class EstadisticasController : Controller
{
    /*PROTECTED REGION ID(EstadisticasController_dependencies) START*/
    private readonly DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly EstadisticasService _estadisticasService;
    private readonly ILogger<EstadisticasController> _logger;
    public EstadisticasController(EstadisticasService estadisticasService, DSM_Odyss.Application.Abstractions.IUnitOfWorkFactory useCaseUnitOfWorkFactory, ILogger<EstadisticasController>? logger = null)
    {
        _estadisticasService = estadisticasService;


        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<EstadisticasController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(EstadisticasController_Index_GET_body) START*/

        var entities = await _estadisticasService.LeerTodasEstadisticasAsync();
        return View(entities.Select(EstadisticasAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(EstadisticasController_Details_GET_body) START*/

        var entity = await _estadisticasService.LeerEstadisticasOIDAsync(id);
        if (entity is null) return NotFound();
        return View(EstadisticasAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(EstadisticasController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.EstadisticasViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.EstadisticasViewModel model, int p_cliente_oid)
    {
    /*PROTECTED REGION ID(EstadisticasController_Create_POST_body) START*/

        ModelState.Remove(nameof(model.Anyo)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.PaisesVisitados)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.FotosSubidas)); // Campo fuera del contrato Create de Application.
        ModelState.Remove(nameof(model.DestinoMasVisitado)); // Campo fuera del contrato Create de Application.

        if (!ModelState.IsValid) return View(model);

        try
        {
            await _estadisticasService.CrearEstadisticasAsync(p_cliente_oid);
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
    /*PROTECTED REGION ID(EstadisticasController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(EstadisticasController_Edit_GET_body) START*/

        var entity = await _estadisticasService.LeerEstadisticasOIDAsync(id);
        if (entity is null) return NotFound();
        return View(EstadisticasAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.EstadisticasViewModel model, int p_cliente_oid)
    {
    /*PROTECTED REGION ID(EstadisticasController_Edit_POST_body) START*/

        ModelState.Remove(nameof(model.Anyo)); // Campo fuera del contrato Edit de Application.
        ModelState.Remove(nameof(model.PaisesVisitados)); // Campo fuera del contrato Edit de Application.
        ModelState.Remove(nameof(model.FotosSubidas)); // Campo fuera del contrato Edit de Application.
        ModelState.Remove(nameof(model.DestinoMasVisitado)); // Campo fuera del contrato Edit de Application.
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _estadisticasService.ModificarEstadisticasAsync(id, p_cliente_oid);
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
    /*PROTECTED REGION ID(EstadisticasController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(EstadisticasController_Delete_GET_body) START*/

        var entity = await _estadisticasService.LeerEstadisticasOIDAsync(id);
        if (entity is null) return NotFound();
        return View(EstadisticasAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(EstadisticasController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.EstadisticasViewModel());
        await _estadisticasService.BorrarEstadisticasAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_DelAnyo_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DelAnyo(int p_anyo)
    {
    /*PROTECTED REGION ID(EstadisticasController_DelAnyo_GET_body) START*/

        var result = await _estadisticasService.DelAnyoAsync(p_anyo);
        return Ok(result.Select(EstadisticasAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
