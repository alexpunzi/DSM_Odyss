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
    /*PROTECTED REGION ID(EstadisticasController_Index_GET_body) ENABLED START*/
        int anyoObjetivo = anyo ?? DateTime.Now.Year;
        var (cliente, estadisticas, metricas) = await ObtenerOCrearEstadisticasClienteActualAsync(anyoObjetivo);

        if (cliente is null || estadisticas is null || metricas is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Recalcula automáticamente para que los datos estén siempre al día
        await _estadisticasService.GenerarPassportRecapAsync(estadisticas.Id, anyoObjetivo);
        estadisticas = await _estadisticasService.LeerEstadisticasOIDAsync(estadisticas.Id) ?? estadisticas;
        var paisesColoreados = await _estadisticasService.ObtenerPaisesColoreadosAsync(estadisticas.Id);

        var viewModel = EstadisticasAssembler.ToDashboardViewModel(
            estadisticas,
            paisesColoreados,
            metricas);

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
        var (cliente, estadisticas, metricas) = await ObtenerOCrearEstadisticasClienteActualAsync(anyoObjetivo);

        if (cliente is null || estadisticas is null || metricas is null)
        {
            return RedirectToAction("Login", "Account");
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
        var (cliente, estadisticas, metricas) = await ObtenerOCrearEstadisticasClienteActualAsync(DateTime.Now.Year);
        if (cliente is null || estadisticas is null || metricas is null)
        {
            return Unauthorized();
        }

        var paises = await _estadisticasService.ObtenerPaisesColoreadosAsync(estadisticas.Id);
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(paises);
        return View(paises.ToList());
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
        var anyo = p_anyo > 0 ? p_anyo : DateTime.Now.Year;
        var result = await _estadisticasService.DelAnyoAsync(anyo);
        var items = result.Select(EstadisticasAssembler.ToViewModel).ToList();
        if (DSM_Odyss.Web.Utils.ControllerJsonOrViewExtensions.WantsJson(this)) return Ok(items);
        return View(new DSM_Odyss.Web.ViewModels.ConsultaResultadoViewModel<int, DSM_Odyss.Web.ViewModels.EstadisticasViewModel>
        {
            Parametro = anyo,
            Resultados = items
        });
    /*PROTECTED REGION END*/
    }

        /*PROTECTED REGION ID(EstadisticasController_custom_helpers) ENABLED START*/
    public sealed record EstadisticasMetricasCalculadas(
        string NombreUsuario,
        List<int> AnyosDisponibles,
        int TotalViajes,
        int ArchivosAlmacenados,
        List<string> TopDestinos,
        double PuntuacionMediaResenas);

    private async Task<(Cliente? cliente, Estadisticas? estadisticas, EstadisticasMetricasCalculadas? metricas)>
    ObtenerOCrearEstadisticasClienteActualAsync(int anyo)
    {
        await using var uow = _useCaseUnitOfWorkFactory.Create();
        var clientes = await uow.ClienteRepository.ReadAllAsync();

        // Al iniciar sesión, HARIA guarda el Id del cliente en User.Identity.Name.
        Cliente? cliente = null;
        if (int.TryParse(User.Identity?.Name, out int clienteId))
        {
            cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        }

        if (cliente is null)
        {
            return (null, null, null);
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

        var viajesCliente = cliente.Viajes?.ToList() ?? new List<Viaje>();
        var anyosDisponibles = viajesCliente
            .Select(v => v.FechaInicio.Year)
            .Append(DateTime.Now.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToList();

        var viajesFiltrados = viajesCliente
            .Where(v => v.FechaInicio.Year == anyo)
            .ToList();

        int archivosAlmacenados = viajesFiltrados
            .SelectMany(v => v.Carpetas?.Multimedia ?? Enumerable.Empty<Multimedia>())
            .Count();

        var topDestinos = viajesFiltrados
            .SelectMany(v => v.ParadasDestino ?? Enumerable.Empty<ParadaDestino>())
            .Where(pd => pd.Destino != null)
            .Select(pd => string.IsNullOrWhiteSpace(pd.Destino.Ciudad)
                ? pd.Destino.Pais
                : $"{pd.Destino.Ciudad} ({pd.Destino.Pais})")
            .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
            .GroupBy(nombre => nombre, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(3)
            .Select(g => g.Key)
            .ToList();

        // Valoración media: solo las reseñas RECIBIDAS en mis viajes.
        var puntuaciones = viajesFiltrados
            .SelectMany(v => v.Resenas ?? Enumerable.Empty<Resena>())
            .Select(r => (double)r.Puntuacion)
            .ToList();

        double puntuacionMedia = puntuaciones.Count > 0
            ? Math.Round(puntuaciones.Average(), 1)
            : 0.0;

        string nombreUsuario = string.IsNullOrWhiteSpace(cliente.User) ? cliente.Email : cliente.User;

        var metricas = new EstadisticasMetricasCalculadas(
            NombreUsuario: nombreUsuario,
            AnyosDisponibles: anyosDisponibles,
            TotalViajes: viajesFiltrados.Count,
            ArchivosAlmacenados: archivosAlmacenados,
            TopDestinos: topDestinos,
            PuntuacionMediaResenas: puntuacionMedia);

        return (cliente, estadisticas, metricas);
    }
    /*PROTECTED REGION END*/
}