// HARIA-WEB MVC managed controller
/*PROTECTED REGION ID(EstadisticasController_usings) START*/
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Application.Abstractions;
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
    private readonly IUnitOfWorkFactory _useCaseUnitOfWorkFactory;
    private readonly EstadisticasService _estadisticasService;
    private readonly ILogger<EstadisticasController> _logger;

    public EstadisticasController(
        EstadisticasService estadisticasService,
        IUnitOfWorkFactory useCaseUnitOfWorkFactory,
        ILogger<EstadisticasController>? logger = null)
    {
        _estadisticasService = estadisticasService;
        _useCaseUnitOfWorkFactory = useCaseUnitOfWorkFactory;
        _logger = logger ?? global::Microsoft.Extensions.Logging.Abstractions.NullLogger<EstadisticasController>.Instance;
    }
    /*PROTECTED REGION END*/

    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_Index_GET_attributes) START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> Index(int? anyo = null)
    {
    /*PROTECTED REGION ID(EstadisticasController_Index_GET_body) START*/
        int anyoObjetivo = anyo ?? DateTime.Now.Year;
        var (cliente, estadisticas) = await ObtenerOCrearEstadisticasClienteActualAsync(anyoObjetivo);

        if (cliente is null || estadisticas is null)
        {
            return RedirectToAction("Login", "Cliente");
        }

        // Recalcula automáticamente para que los datos estén siempre al día
        await _estadisticasService.GenerarPassportRecapAsync(estadisticas.Id, anyoObjetivo);
        estadisticas = await _estadisticasService.LeerEstadisticasOIDAsync(estadisticas.Id) ?? estadisticas;
        var paisesColoreados = await _estadisticasService.ObtenerPaisesColoreadosAsync(estadisticas.Id);

        var viewModel = EstadisticasAssembler.ToDashboardViewModel(
            estadisticas,
            cliente,
            paisesColoreados,
            anyoObjetivo);

        if (TempData["PassportRecapMsg"] is string msg)
        {
            viewModel.MensajePassportRecap = msg;
        }

        return View(viewModel);
    /*PROTECTED REGION END*/
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    /*PROTECTED REGION ID(EstadisticasController_PassportRecap_POST_attributes) ENABLED START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PassportRecap(int anyo)
    {
    /*PROTECTED REGION ID(EstadisticasController_PassportRecap_POST_body) ENABLED START*/
        int anyoObjetivo = anyo > 0 ? anyo : DateTime.Now.Year;
        var (cliente, estadisticas) = await ObtenerOCrearEstadisticasClienteActualAsync(anyoObjetivo);

        if (cliente is null || estadisticas is null)
        {
            return RedirectToAction("Login", "Cliente");
        }

        string resumen = await _estadisticasService.GenerarPassportRecapAsync(estadisticas.Id, anyoObjetivo);
        TempData["PassportRecapMsg"] = resumen;

        return RedirectToAction(nameof(Index), new { anyo = anyoObjetivo });
    /*PROTECTED REGION END*/
    }

    [HttpGet]
    /*PROTECTED REGION ID(EstadisticasController_PaisesColoreados_GET_attributes) ENABLED START*/
    /*PROTECTED REGION END*/
    public async Task<IActionResult> PaisesColoreados()
    {
    /*PROTECTED REGION ID(EstadisticasController_PaisesColoreados_GET_body) ENABLED START*/
        var (cliente, estadisticas) = await ObtenerOCrearEstadisticasClienteActualAsync(DateTime.Now.Year);
        if (cliente is null || estadisticas is null)
        {
            return Unauthorized();
        }

        var paises = await _estadisticasService.ObtenerPaisesColoreadosAsync(estadisticas.Id);
        return Ok(paises);
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
        ModelState.Remove(nameof(model.Anyo));
        ModelState.Remove(nameof(model.PaisesVisitados));
        ModelState.Remove(nameof(model.FotosSubidas));
        ModelState.Remove(nameof(model.DestinoMasVisitado));

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
        ModelState.Remove(nameof(model.Anyo));
        ModelState.Remove(nameof(model.PaisesVisitados));
        ModelState.Remove(nameof(model.FotosSubidas));
        ModelState.Remove(nameof(model.DestinoMasVisitado));
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

    /*PROTECTED REGION ID(EstadisticasController_custom_helpers) ENABLED START*/
    private async Task<(Cliente? cliente, Estadisticas? estadisticas)> ObtenerOCrearEstadisticasClienteActualAsync(int anyo)
    {
        await using var uow = _useCaseUnitOfWorkFactory.Create();
        var clientes = await uow.ClienteRepository.ReadAllAsync();

        string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("Id")
                              ?? User.FindFirstValue("ClienteId");
        string? emailOrName = User.FindFirstValue(ClaimTypes.Email)
                              ?? User.FindFirstValue(ClaimTypes.Name)
                              ?? User.Identity?.Name;

        Cliente? cliente = null;
        if (int.TryParse(userIdClaim, out int clienteId))
        {
            cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        }

        if (cliente is null && !string.IsNullOrWhiteSpace(emailOrName))
        {
            cliente = clientes.FirstOrDefault(c =>
                string.Equals(c.Email, emailOrName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.User, emailOrName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.Nombre, emailOrName, StringComparison.OrdinalIgnoreCase));
        }

        cliente ??= clientes.FirstOrDefault(c => c.Viajes.Any()) ?? clientes.FirstOrDefault();

        if (cliente is null)
        {
            return (null, null);
        }

        var estadisticas = cliente.Estadisticas.FirstOrDefault(e => e.Anyo == anyo)
                           ?? cliente.Estadisticas.FirstOrDefault();

        if (estadisticas is null)
        {
            estadisticas = new Estadisticas
            {
                Anyo = anyo,
                PaisesVisitados = 0,
                FotosSubidas = 0,
                DestinoMasVisitado = "Sin destinos registrados"
            };

            await uow.EstadisticasRepository.SaveAsync(estadisticas);
            cliente.Estadisticas.Add(estadisticas);
            await uow.ClienteRepository.SaveAsync(cliente);
            await uow.CommitAsync();
        }

        return (cliente, estadisticas);
    }
    /*PROTECTED REGION END*/
}