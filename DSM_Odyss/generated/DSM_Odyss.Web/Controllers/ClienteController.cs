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
    /*PROTECTED REGION ID(ClienteController_dependencies) ENABLED START*/
    private readonly ClienteService _clienteService;
    private readonly ViajeService _viajeService;
    private readonly ILogger<ClienteController> _logger;
    public ClienteController(ClienteService clienteService, ViajeService viajeService, ILogger<ClienteController>? logger = null)
    {
        _clienteService = clienteService;
        _viajeService = viajeService;
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
    /*PROTECTED REGION ID(ClienteController_Details_GET_body) ENABLED START*/

        // Si llega sin id (por ejemplo, al volver del login), se muestra el perfil del usuario conectado.
        if (id == 0 && int.TryParse(User.Identity?.Name, out var idConectado))
            id = idConectado;

        var entity = await _clienteService.LeerClienteOIDAsync(id);
        if (entity is null) return NotFound();

        // Para saber quién escribió cada reseña (una Resena no guarda su autor).
        var clientes = await _clienteService.LeerTodosClientesAsync();

        var model = new DSM_Odyss.Web.ViewModels.PerfilUsuarioViewModel
        {
            Id = entity.Id,
            Username = entity.User,
            NombreCompleto = $"{entity.Nombre} {entity.Apellidos}".Trim(),
            TotalViajes = entity.Viajes.Count,
            EsPropio = User.Identity?.Name == entity.Id.ToString(),

            // Mis viajes: del más reciente al más antiguo.
            Viajes = entity.Viajes
                .Where(v => v.EsPublico)
                .OrderByDescending(v => v.FechaInicio)
                .Select(v => new DSM_Odyss.Web.ViewModels.PerfilViajeItem
                {
                    Id = v.Id,
                    Titulo = v.Titulo,
                    FechaInicio = v.FechaInicio
                })
                .ToList(),

                        // Reseñas RECIBIDAS: las que otros usuarios han dejado en mis viajes (las 3 últimas).
            Resenas = entity.Viajes
                .SelectMany(v => v.Resenas.Select(r => new { Viaje = v, Resena = r }))
                .OrderByDescending(x => x.Resena.Fecha)
                .Take(3)
                .Select(x => new DSM_Odyss.Web.ViewModels.PerfilResenaItem
                {
                    Id = x.Resena.Id,
                    TituloViaje = x.Viaje.Titulo,
                    Autor = clientes.FirstOrDefault(c => c.Resenas.Any(r => r.Id == x.Resena.Id))?.User ?? "anónimo",
                    Puntuacion = x.Resena.Puntuacion,
                    Comentario = x.Resena.Comentario,
                    FechaViaje = x.Viaje.FechaInicio
                })
                .ToList(),

            // Galería de recuerdos: carpetas de sus viajes + carpetas compartidas con él (sin repetir).
            Carpetas = entity.Viajes
                .Where(v => v.Carpetas is not null)
                .Select(v => v.Carpetas!)
                .Concat(entity.CarpetasCompartidas)
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .Select(c => new DSM_Odyss.Web.ViewModels.PerfilCarpetaItem
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    TotalArchivos = c.Multimedia.Count
                })
                .ToList()
        };

        return View(model);
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
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ClienteViewModel model)
    {
    /*PROTECTED REGION ID(ClienteController_Create_POST_body) ENABLED START*/


        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _clienteService.CrearClienteAsync(model.User, model.Password!, model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios);
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
    public async Task<IActionResult> Register(DSM_Odyss.Web.ViewModels.ClienteViewModel model)
    {
    /*PROTECTED REGION ID(ClienteController_Register_POST_body) ENABLED START*/
        if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(model.Password), "Debes indicar una contraseña.");
        if (!model.DiaNacimiento.HasValue || !model.MesNacimiento.HasValue || !model.AnioNacimiento.HasValue)
            ModelState.AddModelError(nameof(model.FechaNacimiento), "Debes indicar tu fecha de nacimiento.");
        
        // El nombre de usuario y el email no se pueden repetir.
        var existentes = await _clienteService.LeerTodosClientesAsync();
        if (!string.IsNullOrWhiteSpace(model.User) &&
            existentes.Any(c => string.Equals(c.User, model.User.Trim(), StringComparison.OrdinalIgnoreCase)))
            ModelState.AddModelError(nameof(model.User), "Ese nombre de usuario ya está en uso. Elige otro o inicia sesión.");
        if (!string.IsNullOrWhiteSpace(model.Email) &&
            existentes.Any(c => string.Equals(c.Email, model.Email.Trim(), StringComparison.OrdinalIgnoreCase)))
            ModelState.AddModelError(nameof(model.Email), "Ya existe una cuenta con ese email. Inicia sesión.");

        if (!ModelState.IsValid) return View(model);

        if (!ModelState.IsValid) return View(model);
        Cliente createdUser;
        try
        {
            createdUser = await _clienteService.CrearClienteAsync(model.User, model.Password!, model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios);
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
        return RedirectToAction(nameof(Details), new { id = createdUser.Id });
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
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ClienteViewModel model)
    {
    /*PROTECTED REGION ID(ClienteController_Edit_POST_body) ENABLED START*/


        if (!ModelState.IsValid) return View(model);
        var stored = await _clienteService.LeerClienteOIDAsync(id);
        if (stored is null) return NotFound();
        try
        {
            await _clienteService.ModificarClienteAsync(id, model.User, (string.IsNullOrEmpty(model.Password) ? stored.Password : model.Password), model.Nombre, model.Apellidos, model.Email, model.FechaNacimiento, model.CanalNotificacion, model.RecibirRecordatorios);
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
    /*PROTECTED REGION ID(ClienteController_ConRecordatoriosActivos_GET_body) ENABLED START*/

        var result = await _clienteService.ConRecordatoriosActivosAsync();
        var items = result.Select(ClienteAssembler.ToViewModel).ToList();
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(items);
        return View(items);
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ClienteController_ConViajesPublicos_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConViajesPublicos()
    {
    /*PROTECTED REGION ID(ClienteController_ConViajesPublicos_GET_body) ENABLED START*/

        var result = await _clienteService.ConViajesPublicosAsync();
        var items = result.Select(ClienteAssembler.ToViewModel).ToList();
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(items);
        return View(items);
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
