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

    public async Task<IActionResult> Index()
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();

        var viajesPublicos = (await _viajeService.ViajesPublicosAsync()).ToList();
        if (!viajesPublicos.Any())
        {
            viajesPublicos = (await _viajeService.LeerTodosViajesAsync()).ToList();
        }

        var clientes = (await unitOfWork.ClienteRepository.ReadAllAsync()).ToList();

        var viajesVm = viajesPublicos.Select(v =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == v.Id);
            return new ViajePopularItemViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                AutorNombre = cliente?.Email ?? "Viajero",
                ImagenUrl = v.Collages?.UrlImagen
            };
        }).ToList();

        var destinosCatalogo = (await unitOfWork.DestinoRepository.ReadAllAsync()).ToList();
        var paradasDestino = (await unitOfWork.ParadaDestinoRepository.ReadAllAsync()).ToList();

        var conteoPorDestino = paradasDestino
            .Where(p => p.Destino != null)
            .GroupBy(p => p.Destino.Id)
            .ToDictionary(g => g.Key, g => g.Select(p => p.Viaje?.Id ?? 0).Distinct().Count());

        var topDestinosVm = destinosCatalogo
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

        var model = new HomeIndexViewModel
        {
            ViajesPopulares = viajesVm,
            TopDestinosPopulares = topDestinosVm
        };

        return View(model);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}