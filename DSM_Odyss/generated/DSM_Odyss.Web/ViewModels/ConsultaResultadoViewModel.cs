namespace DSM_Odyss.Web.ViewModels;

public sealed class ConsultaResultadoViewModel<TParam, TItem>
{
    public TParam? Parametro { get; set; }
    public IReadOnlyList<TItem> Resultados { get; set; } = Array.Empty<TItem>();
}
