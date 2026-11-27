// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(ResenaController_usings) START*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.Assemblers;

using DSM_Odyss.Domain.Enums;
/*PROTECTED REGION END*/

namespace DSM_Odyss.Web.Controllers;

/*PROTECTED REGION ID(ResenaController_class_attributes) ENABLED START*/
[Authorize]
/*PROTECTED REGION END*/
public partial class ResenaController : Controller
{
    /*PROTECTED REGION ID(ResenaController_dependencies) ENABLED START*/
    private readonly ResenaService _resenaService;
    private readonly ClienteService _clienteService;
    private readonly ViajeService _viajeService;
    private readonly ILogger<ResenaController> _logger;
    public ResenaController(ResenaService resenaService, ClienteService clienteService, ViajeService viajeService, ILogger<ResenaController>? logger = null)
    {
        _resenaService = resenaService;
        _clienteService = clienteService;
        _viajeService = viajeService;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<ResenaController>.Instance;
    }
    /*PROTECTED REGION END*/
    [HttpGet]
    /*PROTECTED REGION ID(ResenaController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index()
    {
    /*PROTECTED REGION ID(ResenaController_Index_GET_body) ENABLED START*/

        // Reseñas RECIBIDAS: todas las que otros usuarios han dejado en mis viajes.
        if (!int.TryParse(User.Identity?.Name, out var clienteId))
            return RedirectToAction("Login", "Account");

        var cliente = await _clienteService.LeerClienteOIDAsync(clienteId);
        if (cliente is null)
            return RedirectToAction("Login", "Account");

        // Para saber quién escribió cada reseña.
        var clientes = await _clienteService.LeerTodosClientesAsync();

        var model = cliente.Viajes
            .SelectMany(v => v.Resenas.Select(r => new { Viaje = v, Resena = r }))
            .OrderByDescending(x => x.Resena.Fecha)
            .Select(x => new DSM_Odyss.Web.ViewModels.PerfilResenaItem
            {
                Id = x.Resena.Id,
                TituloViaje = x.Viaje.Titulo,
                Autor = clientes.FirstOrDefault(c => c.Resenas.Any(r => r.Id == x.Resena.Id))?.User ?? "anónimo",
                Puntuacion = x.Resena.Puntuacion,
                Comentario = x.Resena.Comentario,
                FechaViaje = x.Viaje.FechaInicio
            })
            .ToList();

        return View(model);
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ResenaController_Details_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Details(int id)
    {
    /*PROTECTED REGION ID(ResenaController_Details_GET_body) START*/

        var entity = await _resenaService.LeerResenaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ResenaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ResenaController_Create_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public IActionResult Create()
    {
    /*PROTECTED REGION ID(ResenaController_Create_GET_body) START*/

        return View(new DSM_Odyss.Web.ViewModels.ResenaViewModel());
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ResenaController_Create_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Create(DSM_Odyss.Web.ViewModels.ResenaViewModel model, int p_viaje_oid, int p_cliente_oid)
    {
    /*PROTECTED REGION ID(ResenaController_Create_POST_body) START*/



        if (!ModelState.IsValid) return View(model);

        try
        {
            await _resenaService.CrearResenaAsync(model.Comentario, model.Fecha, model.Puntuacion, p_viaje_oid, p_cliente_oid);
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
    /*PROTECTED REGION ID(ResenaController_Edit_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id)
    {
    /*PROTECTED REGION ID(ResenaController_Edit_GET_body) START*/

        var entity = await _resenaService.LeerResenaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ResenaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ResenaController_Edit_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Edit(int id, DSM_Odyss.Web.ViewModels.ResenaViewModel model, int p_viaje_oid, int p_cliente_oid)
    {
    /*PROTECTED REGION ID(ResenaController_Edit_POST_body) START*/


        if (!ModelState.IsValid) return View(model);

        try
        {
            await _resenaService.ModificarResenaAsync(id, model.Comentario, model.Fecha, model.Puntuacion, p_viaje_oid, p_cliente_oid);
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
    /*PROTECTED REGION ID(ResenaController_Delete_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Delete(int id)
    {
    /*PROTECTED REGION ID(ResenaController_Delete_GET_body) START*/

        var entity = await _resenaService.LeerResenaOIDAsync(id);
        if (entity is null) return NotFound();
        return View(ResenaAssembler.ToViewModel(entity));
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(ResenaController_DeleteConfirmed_POST_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
    /*PROTECTED REGION ID(ResenaController_DeleteConfirmed_POST_body) START*/

        if (!ModelState.IsValid) return View("Delete", new DSM_Odyss.Web.ViewModels.ResenaViewModel());
        await _resenaService.BorrarResenaAsync(id);
        return RedirectToAction(nameof(Index));
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(ResenaController_ConPuntuacionMinima_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> ConPuntuacionMinima(int p_puntuacion)
    {
    /*PROTECTED REGION ID(ResenaController_ConPuntuacionMinima_GET_body) START*/

        var puntuacion = p_puntuacion > 0 ? p_puntuacion : 1;
        var result = await _resenaService.ConPuntuacionMinimaAsync(puntuacion);
        var items = result.Select(ResenaAssembler.ToViewModel).ToList();
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(items);
        return View(new DSM_Odyss.Web.ViewModels.ConsultaResultadoViewModel<int, DSM_Odyss.Web.ViewModels.ResenaViewModel>
        {
            Parametro = puntuacion,
            Resultados = items
        });
    /*PROTECTED REGION END*/
    }
}
