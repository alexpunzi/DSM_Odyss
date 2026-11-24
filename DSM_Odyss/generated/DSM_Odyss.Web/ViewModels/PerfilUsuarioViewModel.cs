namespace DSM_Odyss.Web.ViewModels;

// M08/M16 · Página principal del perfil ("Mi galería").
// De momento solo los datos de la cabecera. Más adelante se añadirán viajes, reseñas y galería.
public class PerfilUsuarioViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int TotalViajes { get; set; }

    // True si el perfil que se ve es el del usuario conectado.
    public bool EsPropio { get; set; }
    // true = solo foto y nombre (M12).
    public bool Compacta { get; set; }
}