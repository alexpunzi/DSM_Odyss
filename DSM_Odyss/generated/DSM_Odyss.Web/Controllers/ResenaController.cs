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

        // Mis reseñas: todas las del usuario conectado, de la más reciente a la más antigua.
        if (!int.TryParse(User.Identity?.Name, out var clienteId))
            return RedirectToAction("Login", "Account");

        var cliente = await _clienteService.LeerClienteOIDAsync(clienteId);
        if (cliente is null)
            return RedirectToAction("Login", "Account");

        // Una Resena no sabe de qué Viaje es: lo buscamos recorriendo los viajes.
        var todosLosViajes = await _viajeService.LeerTodosViajesAsync();
        var viajePorResena = new Dictionary<int, DSM_Odyss.Domain.Entities.Viaje>();
        foreach (var v in todosLosViajes)
            foreach (var r in v.Resenas)
                viajePorResena[r.Id] = v;

        var model = cliente.Resenas
            .OrderByDescending(r => r.Fecha)
            .Select(r =>
            {
                viajePorResena.TryGetValue(r.Id, out var viaje);
                return new DSM_Odyss.Web.ViewModels.PerfilResenaItem
                {
                    Id = r.Id,
                    TituloViaje = viaje?.Titulo ?? "Viaje desconocido",
                    Puntuacion = r.Puntuacion,
                    FechaViaje = viaje?.FechaInicio ?? r.Fecha,
                    Comentario = r.Comentario
                };
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

        var result = await _resenaService.ConPuntuacionMinimaAsync(p_puntuacion);
        return Ok(result.Select(ResenaAssembler.ToViewModel).ToList());
    /*PROTECTED REGION END*/
    }
}
