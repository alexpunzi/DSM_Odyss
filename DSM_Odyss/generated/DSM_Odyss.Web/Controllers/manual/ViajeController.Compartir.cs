// Código manual (no generado por HARIA): CU-12 Compartir itinerario.
// Imita la compartición de carpetas (CarpetaController.Features.cs): buscar usuario → compartir → quitar.
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.Controllers;

public partial class ViajeController
{
    // GET /Viaje/Compartir/5?q=ana → personas con acceso + buscador de usuarios
    [HttpGet]
    public async Task<IActionResult> Compartir(int id, string? q)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return Challenge();

        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var viaje = await unitOfWork.ViajeRepository.ReadAsync(id);
        if (viaje is null) return NotFound();

        var clientes = (await unitOfWork.ClienteRepository.ReadAllAsync()).ToList();
        var autor = clientes.FirstOrDefault(c => c.Viajes.Any(v => v.Id == id));
        if (autor is null || autor.Id != clienteId) return Forbid();   // solo el autor comparte

        var compartidosIds = viaje.CompartidoCon.Select(c => c.Id).ToHashSet();

        var model = new CompartirViajeViewModel
        {
            ViajeId = viaje.Id,
            Titulo = viaje.Titulo,
            EsPublico = viaje.EsPublico,
            Busqueda = q,
            Compartidos = viaje.CompartidoCon
                .OrderBy(c => c.User)
                .Select(ToItem)
                .ToList()
        };

        // Búsqueda por usuario o nombre (mínimo 2 letras), sin el autor ni los que ya tienen acceso
        var texto = q?.Trim();
        if (!string.IsNullOrEmpty(texto) && texto.Length >= 2)
        {
            model.ResultadosBusqueda = clientes
                .Where(c => c.Id != clienteId && !compartidosIds.Contains(c.Id))
                .Where(c => c.User.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                            $"{c.Nombre} {c.Apellidos}".Contains(texto, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.User)
                .Take(10)
                .Select(ToItem)
                .ToList();
        }

        return View(model);
    }

    // POST: da acceso a un usuario (operación custom CompartirConCliente)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompartirCon(int viajeId, int clienteId)
    {
        if (!await EsAutorDelViajeAsync(viajeId)) return Forbid();
        if (int.TryParse(User.Identity?.Name, out var yo) && yo == clienteId)
            return BadRequest("No puedes compartir un viaje contigo.");

        await _viajeService.CompartirConClienteAsync(viajeId, new[] { clienteId });
        TempData["CompartirMsg"] = "Viaje compartido.";
        return RedirectToAction(nameof(Compartir), new { id = viajeId });
    }

    // POST: quita el acceso (operación custom DejarDeCompartirConCliente)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DejarDeCompartir(int viajeId, int clienteId)
    {
        if (!await EsAutorDelViajeAsync(viajeId)) return Forbid();

        await _viajeService.DejarDeCompartirConClienteAsync(viajeId, new[] { clienteId });
        TempData["CompartirMsg"] = "Acceso retirado.";
        return RedirectToAction(nameof(Compartir), new { id = viajeId });
    }

    private async Task<bool> EsAutorDelViajeAsync(int viajeId)
    {
        if (!int.TryParse(User.Identity?.Name, out var clienteId)) return false;
        await using var unitOfWork = _formUnitOfWorkFactory.Create();
        var cliente = await unitOfWork.ClienteRepository.ReadAsync(clienteId);
        return cliente?.Viajes.Any(v => v.Id == viajeId) == true;
    }

    private static UsuarioCompartirItem ToItem(Cliente c) => new()
    {
        Id = c.Id,
        Usuario = c.User,
        NombreCompleto = $"{c.Nombre} {c.Apellidos}".Trim()
    };
}