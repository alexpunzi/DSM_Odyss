// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ClienteController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;
using DSM_Odyss.Web.Utils.Security;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ClienteController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ClienteController : Controller
{
    /*PROTECTED REGION ID(ClienteController_dependencies) START*/
    private readonly ClienteService _clienteService;
    private readonly ILogger<ClienteController> _logger;
    public ClienteController(ClienteService clienteService, ILogger<ClienteController>? logger = null)
    {
        _clienteService = clienteService;



        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ClienteController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ClienteController_Index_GET_body) START*/

        var entities = await _clienteService.LeerTodosClientesAsync();
        return View(entities.Select(ClienteAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ClienteController_Details_GET_body) START*/

        var entity = await _clienteService.LeerClienteOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ClienteAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(ClienteController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.ClienteViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ClienteViewModel model, int p_checklist_oid, int p_recordatorio_oid)
    {
    /*PROTECTED REGION ID(ClienteController_Create_POST_body) START*/


        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _clienteService.CrearClienteAsync(model.User, model.Password!, model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios, p_checklist_oid, p_recordatorio_oid);
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
    /*PROTECTED REGION ID(ClienteController_Register_GET_attributes) START*/
    [AllowAnonymous]
    /*PROTECTED REGION END*/
    public IActionResult Register()
    {
    /*PROTECTED REGION ID(ClienteController_Register_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.ClienteViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_Register_POST_attributes) START*/
    [AllowAnonymous]
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Register(DSM_Odyss.Web.ViewModels.ClienteViewModel model, int p_checklist_oid, int p_recordatorio_oid)
    {
    /*PROTECTED REGION ID(ClienteController_Register_POST_body) START*/


        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!ModelState.IsValid) return View(model);
        Cliente createdUser;
        try
        {
            createdUser = await _clienteService.CrearClienteAsync(model.User, model.Password!, model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios, p_checklist_oid, p_recordatorio_oid);
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "Could not create the record.");
            ModelState.AddModelError(string.Empty, "No se ha podido guardar el registro. Inténtalo de nuevo.");
            return View(model);
        }
        try
        {
            await HariaWebAuthentication.SignInAsync(HttpContext, "Cliente", createdUser.Id, createdUser);
            HttpContext.Session.SetUser(ClienteAssembler.ToViewModel(createdUser));
        }
        catch (Exception error) when (error is not OperationCanceledException)
        {
            _logger.LogError(error, "La cuenta se ha creado, pero no se ha podido iniciar la sesión.");
            ModelState.AddModelError(string.Empty, "Tu cuenta ya está creada. Inicia sesión desde la página de acceso.");
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ClienteController_Edit_GET_body) START*/

        var entity = await _clienteService.LeerClienteOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ClienteAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ClienteViewModel model, int p_checklist_oid, int p_recordatorio_oid)
    {
    /*PROTECTED REGION ID(ClienteController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);
        var stored = await _clienteService.LeerClienteOIDAsync(id);
        if (stored is null) return NotFound();
        try
        {
            await _clienteService.ModificarClienteAsync(id, model.User, (string.IsNullOrEmpty(model.Password) ? stored.Password : model.Password), model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios, p_checklist_oid, p_recordatorio_oid);
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
    /*PROTECTED REGION ID(ClienteController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ClienteController_Delete_GET_body) START*/

        var entity = await _clienteService.LeerClienteOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ClienteAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ClienteController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ClienteViewModel());
        await _clienteService.BorrarClienteAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_ConRecordatoriosActivos_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConRecordatoriosActivos()
    {
    /*PROTECTED REGION ID(ClienteController_ConRecordatoriosActivos_GET_body) START*/

        var result = await _clienteService.ConRecordatoriosActivosAsync();
        return Ok(result.Select(ClienteAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_ConViajesPublicos_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConViajesPublicos()
    {
    /*PROTECTED REGION ID(ClienteController_ConViajesPublicos_GET_body) START*/

        var result = await _clienteService.ConViajesPublicosAsync();
        return Ok(result.Select(ClienteAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_AgregarContactoRelacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarContactoRelacion(int p_oid, IReadOnlyList<int> p_contactos_oid)
    {
    /*PROTECTED REGION ID(ClienteController_AgregarContactoRelacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.AgregarContactoRelacionAsync(p_oid, p_contactos_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_QuitarContactoRelacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> QuitarContactoRelacion(int p_oid, IReadOnlyList<int> p_contactos_oid)
    {
    /*PROTECTED REGION ID(ClienteController_QuitarContactoRelacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.QuitarContactoRelacionAsync(p_oid, p_contactos_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_AgregarEstadisticasRelacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarEstadisticasRelacion(int p_oid, IReadOnlyList<int> p_estadisticas_oid)
    {
    /*PROTECTED REGION ID(ClienteController_AgregarEstadisticasRelacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.AgregarEstadisticasRelacionAsync(p_oid, p_estadisticas_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_AgregarViajeRelacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarViajeRelacion(int p_oid, IReadOnlyList<int> p_viajes_oid)
    {
    /*PROTECTED REGION ID(ClienteController_AgregarViajeRelacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.AgregarViajeRelacionAsync(p_oid, p_viajes_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ClienteController_AgregarResenaRelacion_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> AgregarResenaRelacion(int p_oid, IReadOnlyList<int> p_resenas_oid)
    {
    /*PROTECTED REGION ID(ClienteController_AgregarResenaRelacion_POST_body) START*/

        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _clienteService.AgregarResenaRelacionAsync(p_oid, p_resenas_oid);
        return NoContent();
    /*PROTECTED REGION END*/
    }
}
