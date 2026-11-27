using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.Controllers;

public sealed class HomeController : Controller
{
    private readonly ViajeService _viajeService;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public HomeController(ViajeService viajeService, IUnitOfWorkFactory unitOfWorkFactory)
    {
        _viajeService = viajeService;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IActionResult> Index(string? q)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();

        var viajesPublicos = (await _viajeService.ViajesPublicosAsync()).ToList();

        var clientes = (await unitOfWork.ClienteRepository.ReadAllAsync()).ToList();

        var busqueda = q?.Trim();
        var viajesFiltrados = string.IsNullOrWhiteSpace(busqueda)
            ? viajesPublicos
            : viajesPublicos
                .Where(v => !string.IsNullOrWhiteSpace(v.Titulo) &&
                            v.Titulo.Contains(busqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var viajesVm = viajesFiltrados.Select(v =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Viajes.Any(viaje => viaje.Id == v.Id));
            var autorNombre = cliente is null
                ? string.Empty
                : $"{cliente.Nombre} {cliente.Apellidos}".Trim();

            return new ViajePopularItemViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                AutorNombre = string.IsNullOrWhiteSpace(autorNombre) ? "Viajero" : autorNombre,
                ImagenUrl = v.Collages?.UrlImagen
            };
        }).ToList();

        var destinosCatalogo = (await unitOfWork.DestinoRepository.ReadAllAsync()).ToList();
        var paradasDestino = (await unitOfWork.ParadaDestinoRepository.ReadAllAsync()).ToList();
        var viajeIdsPublicos = viajesPublicos.Select(v => v.Id).ToHashSet();

        var conteoPorDestino = paradasDestino
            .Where(p => p.Destino != null && p.Viaje != null && viajeIdsPublicos.Contains(p.Viaje.Id))
            .GroupBy(p => p.Destino.Id)
            .ToDictionary(g => g.Key, g => g.Select(p => p.Viaje!.Id).Distinct().Count());

        var topDestinosVm = destinosCatalogo
            .Where(d => conteoPorDestino.ContainsKey(d.Id))
            .Select(d =>
            {
                conteoPorDestino.TryGetValue(d.Id, out var numViajes);
                var nombreDestino = !string.IsNullOrWhiteSpace(d.Ciudad) && !string.IsNullOrWhiteSpace(d.Pais)
                    ? $"{d.Ciudad}, {d.Pais}"
                    : (!string.IsNullOrWhiteSpace(d.Ciudad) ? d.Ciudad : (d.Pais ?? "Destino"));

                return new DestinoPopularItemViewModel
                {
                    Id = d.Id,
                    Nombre = nombreDestino,
                    Detalles = !string.IsNullOrWhiteSpace(d.Pais) ? $"Destino en {d.Pais}" : "Lugar del catálogo",
                    TotalViajeros = numViajes
                };
            })
            .OrderByDescending(x => x.TotalViajeros)
            .ThenBy(x => x.Nombre)
            .Take(10)
            .ToList();

        ViewData["BusquedaViajes"] = busqueda;
        var model = new HomeIndexViewModel
        {
            Busqueda = busqueda,
            ViajesPopulares = viajesVm,
            TopDestinosPopulares = topDestinosVm
        };

        return View(model);
    }

    public IActionResult Privacy() => View();

    // Página con enlaces a todas las consultas (operaciones readFilter) de la aplicación.
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Consultas() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}