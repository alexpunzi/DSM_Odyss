namespace DSM_Odyss.Web.ViewModels;

// Datos de la cabecera común del perfil (M08–M18).
public class PerfilCabeceraViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TotalViajes { get; set; }

    // "galeria", "mapa", "itinerarios", "estadisticas" o null (sin barra de pestañas).
    public string? PestanaActiva { get; set; }
}