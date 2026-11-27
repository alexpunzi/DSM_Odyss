using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public class EstadisticasViewModel
{
    public int Id { get; set; }

    [Display(Name = "Año")]
    public int? Anyo { get; set; } = DateTime.Now.Year;

    [Display(Name = "Países visitados")]
    public int? PaisesVisitados { get; set; }

    [Display(Name = "Fotos subidas")]
    public int? FotosSubidas { get; set; }

    [Display(Name = "Destino más visitado")]
    public string? DestinoMasVisitado { get; set; }

    // --- Propiedades extendidas para RF-05, RF-11, RF-12 y Mockup M13 ---
    public string NombreUsuario { get; set; } = string.Empty;
    public int TotalViajes { get; set; }
    public int ArchivosAlmacenados { get; set; }
    public double PuntuacionMediaResenas { get; set; }
    public List<string> TopDestinos { get; set; } = new();
    public List<string> PaisesColoreados { get; set; } = new();
    public List<int> AnyosDisponibles { get; set; } = new();
    public string? MensajePassportRecap { get; set; }
}