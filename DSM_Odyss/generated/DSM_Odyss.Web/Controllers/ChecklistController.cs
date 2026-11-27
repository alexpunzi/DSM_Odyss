// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ChecklistController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;
using DSM_Odyss.Web.ViewModels;
using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ChecklistController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ChecklistController : Controller
{
    /*PROTECTED REGION ID(ChecklistController_dependencies) ENABLED START*/
    private readonly ChecklistService _checklistService;
    private readonly TareaService _tareaService;
    private readonly ClienteService _clienteService;
    private readonly ILogger<ChecklistController> _logger;

    public ChecklistController(ChecklistService checklistService, TareaService tareaService, ClienteService clienteService, ILogger<ChecklistController>? logger = null)
    {
        _checklistService = checklistService;
        _tareaService = tareaService;
        _clienteService = clienteService;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ChecklistController>.Instance;
    }
    /*PROTECTED REGION END*/

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ChecklistController_Index_GET_body) ENABLED START*/
        // Solo los checklists del usuario conectado.
        var cliente = await ClienteConectadoAsync();
        if (cliente is null) return RedirectToAction("Login", "Account");
        return View(cliente.Checklists.Select(ChecklistAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_Details_GET_body) ENABLED START*/
        if (!await EsMiChecklistAsync(id)) return NotFound();
        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();

        var viewModel = ChecklistAssembler.ToViewModel(entity);
        var conTareas = (await _checklistService.ConTareasAsync()).FirstOrDefault(c => c.Id == id);
        if (conTareas?.Tareas != null)
        {
            viewModel.Tareas = conTareas.Tareas.Select(t => new TareaItemViewModel
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
                Completada = t.Completada
            }).ToList();
        }

        return View(viewModel);
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ChecklistController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(ChecklistController_Create_GET_body) START*/
        return View(new ChecklistViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(ChecklistViewModel model)
    {
    /*PROTECTED REGION ID(ChecklistController_Create_POST_body) ENABLED START*/
        if (!ModelState.IsValid) return View(model);
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return RedirectToAction("Login", "Account");

        try
        {
            // El checklist pertenece al usuario que lo crea.
            var creada = await _checklistService.CrearChecklistAsync(model.Nombre, clienteId);
            int checklistId = creada.Id;

            if (model.Tareas != null)
            {
                foreach (var tarea in model.Tareas.Where(t => !string.IsNullOrWhiteSpace(t.Descripcion)))
                {
                    await _tareaService.CrearTareaAsync(tarea.Descripcion, tarea.Completada, checklistId);
                }
            }
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
    /*PROTECTED REGION ID(ChecklistController_Edit_GET_body) ENABLED START*/
        if (!await EsMiChecklistAsync(id)) return NotFound();
        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();

        var viewModel = ChecklistAssembler.ToViewModel(entity);
        var conTareas = (await _checklistService.ConTareasAsync()).FirstOrDefault(c => c.Id == id);
        if (conTareas?.Tareas != null)
        {
            viewModel.Tareas = conTareas.Tareas.Select(t => new TareaItemViewModel
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
                Completada = t.Completada
            }).ToList();
        }

        return View(viewModel);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, ChecklistViewModel model)
    {
    /*PROTECTED REGION ID(ChecklistController_Edit_POST_body) ENABLED START*/
        if (!await EsMiChecklistAsync(id)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _checklistService.ModificarChecklistAsync(id, model.Nombre);

            var conTareas = (await _checklistService.ConTareasAsync()).FirstOrDefault(c => c.Id == id);
            var tareasActualesIds = conTareas?.Tareas?.Select(t => t.Id).ToList() ?? new List<int>();

            var tareasFormulario = model.Tareas?
                .Where(t => !string.IsNullOrWhiteSpace(t.Descripcion))
                .ToList() ?? new List<TareaItemViewModel>();

            // 1. Eliminar las tareas que el usuario quitó de la lista
            var idsEnFormulario = tareasFormulario.Where(t => t.Id > 0).Select(t => t.Id).ToHashSet();
            foreach (var idBorrar in tareasActualesIds.Where(tid => !idsEnFormulario.Contains(tid)))
            {
                await _tareaService.BorrarTareaAsync(idBorrar);
            }

            // 2. Actualizar las existentes o crear las nuevas
            foreach (var tarea in tareasFormulario)
            {
                if (tarea.Id > 0)
                {
                    await _tareaService.ModificarTareaAsync(tarea.Id, tarea.Descripcion, tarea.Completada, id);
                }
                else
                {
                    await _tareaService.CrearTareaAsync(tarea.Descripcion, tarea.Completada, id);
                }
            }
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
    /*PROTECTED REGION ID(ChecklistController_Delete_GET_body) ENABLED START*/
        if (!await EsMiChecklistAsync(id)) return NotFound();
        var entity = await _checklistService.LeerChecklistOIDAsync(id);
        if (entity is null) return NotFound();

        var viewModel = ChecklistAssembler.ToViewModel(entity);
        var conTareas = (await _checklistService.ConTareasAsync()).FirstOrDefault(c => c.Id == id);
        if (conTareas?.Tareas != null)
        {
            viewModel.Tareas = conTareas.Tareas.Select(t => new TareaItemViewModel
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
                Completada = t.Completada
            }).ToList();
        }

        return View(viewModel);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ChecklistController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ChecklistController_DeleteConfirmed_POST_body) ENABLED START*/
        if (!await EsMiChecklistAsync(id)) return NotFound();
        if (!ModelState.IsValid) return View("Delete", new ChecklistViewModel());

        var conTareas = (await _checklistService.ConTareasAsync()).FirstOrDefault(c => c.Id == id);
        if (conTareas?.Tareas != null)
        {
            foreach (var tarea in conTareas.Tareas)
            {
                await _tareaService.BorrarTareaAsync(tarea.Id);
            }
        }

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
        var items = result.Select(ChecklistAssembler.ToViewModel).ToList();
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(items);
        return View(items);
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
}