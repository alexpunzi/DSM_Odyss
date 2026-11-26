namespace DSM_Odyss.Web.ViewModels;

// M08/M16 · Página principal del perfil ("Mi galería").
public class PerfilUsuarioViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int TotalViajes { get; set; }
    public bool EsPropio { get; set; }

    public List<PerfilViajeItem> Viajes { get; set; } = new();       // Mis viajes
    public List<PerfilResenaItem> Resenas { get; set; } = new();     // Reseñas recientes
    public List<PerfilCarpetaItem> Carpetas { get; set; } = new();   // Galería de recuerdos
}

public class PerfilViajeItem
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
}

public class PerfilResenaItem
{
    public int Id { get; set; }
    public string TituloViaje { get; set; } = string.Empty;
    public int Puntuacion { get; set; }        // de 0 a 5
    public DateTime FechaViaje { get; set; }
    public string Comentario { get; set; } = string.Empty;
}

public class PerfilCarpetaItem
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int TotalArchivos { get; set; }
}