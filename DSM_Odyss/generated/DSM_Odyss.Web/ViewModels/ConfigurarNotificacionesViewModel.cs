using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Web.ViewModels;

// Preferencias de notificación del usuario conectado (CU-14).
public class ConfigurarNotificacionesViewModel
{
    public int ClienteId { get; set; }
    public CanalNotificacion Canal { get; set; } = CanalNotificacion.email;
    public bool RecibirRecordatorios { get; set; }
}