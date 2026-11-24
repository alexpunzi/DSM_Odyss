namespace DSM_Odyss.Web.ViewModels;

// Datos de la cabecera común del perfil (M08–M18).
public class PerfilCabeceraViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TotalViajes { get; set; }

    // Recordatorios pendientes del usuario (globo rojo de la campana).
    public int NotificacionesPendientes { get; set; }

    // "galeria", "mapa", "itinerarios", "estadisticas" o null (sin barra de pestañas).
    public string? PestanaActiva { get; set; }

    // true = solo foto y nombre, sin nº de viajes ni campana (M12).
    public bool Compacta { get; set; }
}