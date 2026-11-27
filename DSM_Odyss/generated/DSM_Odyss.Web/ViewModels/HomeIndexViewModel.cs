namespace DSM_Odyss.Web.ViewModels;

public sealed class HomeIndexViewModel
{
    public string? Busqueda { get; set; }
    public List<ViajePopularItemViewModel> ViajesPopulares { get; set; } = new();
    public List<DestinoPopularItemViewModel> TopDestinosPopulares { get; set; } = new();
}

public sealed class ViajePopularItemViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string AutorNombre { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
}

public sealed class DestinoPopularItemViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Detalles { get; set; }
    public int TotalViajeros { get; set; }
}