namespace DSM_Odyss.Web.ViewModels;

public sealed class CarpetaDetailsViewModel
{
    public int Id { get; set; }
    public int ViajeId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ViajeTitulo { get; set; } = string.Empty;
    public bool PuedeEditar { get; set; }
    public bool EsPropietario { get; set; }
    public bool EsEnlacePublico { get; set; }
    public string? EnlacePublico { get; set; }
    public string? Busqueda { get; set; }
    public List<CarpetaMultimediaItemViewModel> Archivos { get; set; } = new();
    public List<CarpetaCompartidoItemViewModel> Compartidos { get; set; } = new();
    public List<CarpetaUsuarioItemViewModel> ResultadosBusqueda { get; set; } = new();
}

public sealed class CarpetaMultimediaItemViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsVideo { get; set; }
    public bool TieneArchivo { get; set; }
    public string Url { get; set; } = string.Empty;
}

public sealed class CarpetaCompartidoItemViewModel
{
    public int Id { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public bool PuedeEditar { get; set; }
}

public sealed class CarpetaUsuarioItemViewModel
{
    public int Id { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
}
