using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.ViewComponents;

// Cabecera común del perfil. Carga sola los datos del usuario conectado,
// así ningún controlador tiene que preocuparse de ella.
public class PerfilCabeceraViewComponent : ViewComponent
{
    private readonly ClienteService _clienteService;

    public PerfilCabeceraViewComponent(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? pestana = null, bool compacta = false)
    {
        // Al iniciar sesión, HARIA guarda el Id del cliente en User.Identity.Name.
        if (!int.TryParse(User.Identity?.Name, out var id))
            return Content(string.Empty);

        var cliente = await _clienteService.LeerClienteOIDAsync(id);
        if (cliente is null)
            return Content(string.Empty);

        // Recordatorios del usuario que todavía no se han enviado.
        var misPendientes = cliente.Recordatorios.Count(r => !r.Enviado);

        var model = new PerfilCabeceraViewModel
        {
            Id = cliente.Id,
            Username = cliente.User,
            TotalViajes = cliente.Viajes.Count,
            NotificacionesPendientes = misPendientes,
            PestanaActiva = pestana,
            Compacta = compacta
        };

        return View(model);
    }
}