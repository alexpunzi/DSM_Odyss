// Código manual: lista de viajes que otros usuarios han compartido conmigo (CU-12).
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.ViewComponents;

public class ViajesCompartidosViewComponent : ViewComponent
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public ViajesCompartidosViewComponent(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = new List<ViajeCompartidoItem>();
        if (!int.TryParse(UserClaimsPrincipal.Identity?.Name, out var clienteId))
            return View(items);

        await using var unitOfWork = _unitOfWorkFactory.Create();
        var viajes = await unitOfWork.ViajeRepository.ReadAllAsync();
        var clientes = await unitOfWork.ClienteRepository.ReadAllAsync();

        items = viajes
            .Where(v => v.CompartidoCon.Any(c => c.Id == clienteId))
            .OrderByDescending(v => v.FechaInicio)
            .Select(v => new ViajeCompartidoItem
            {
                Id = v.Id,
                Titulo = v.Titulo,
                FechaInicio = v.FechaInicio,
                Autor = clientes.FirstOrDefault(c => c.Viajes.Any(x => x.Id == v.Id))?.User ?? "desconocido"
            })
            .ToList();

        return View(items);
    }
}