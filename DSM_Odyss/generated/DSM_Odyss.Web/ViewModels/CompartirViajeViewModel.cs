namespace DSM_Odyss.Web.ViewModels;

// CU-12 Compartir itinerario: pantalla para compartir un viaje con otros usuarios.
public class CompartirViajeViewModel
{
    public int ViajeId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool EsPublico { get; set; }
    public string? Busqueda { get; set; }
    public List<UsuarioCompartirItem> ResultadosBusqueda { get; set; } = new();
    public List<UsuarioCompartirItem> Compartidos { get; set; } = new();
}

public class UsuarioCompartirItem
{
    public int Id { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
}

// Tarjeta de un viaje que otro usuario ha compartido contigo.
public class ViajeCompartidoItem
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public string Autor { get; set; } = string.Empty;
}