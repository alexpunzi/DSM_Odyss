using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ViajeViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ViajeViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Titulo { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; }

    public bool EsPublico { get; set; }

    public float Presupuesto { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.TipoViaje), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.TipoViaje TipoViaje { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.EstadoViaje), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.EstadoViaje Estado { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Carpetas")]
    public int? CarpetasId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CarpetasOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Carpetas")]
    public string? CarpetasLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Collages")]
    public int? CollagesId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CollagesOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Collages")]
    public string? CollagesLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
