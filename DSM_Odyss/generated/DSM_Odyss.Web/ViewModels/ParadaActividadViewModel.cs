using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ParadaActividadViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ParadaActividadViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Nombre { get; set; } = string.Empty;

    public int Dia { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Horario { get; set; }

    [UIHint("HariaReadOnly:Create,Edit")]
    public string? Detalles { get; set; }

    public bool Completada { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [UIHint("HariaAssociation")]
    [Display(Name = "Viaje")]
    public int? ViajeId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ViajeOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Viaje")]
    public string? ViajeLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Ubicacion")]
    public int? UbicacionId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> UbicacionOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Ubicacion")]
    public string? UbicacionLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "ParadaDestino")]
    public int? ParadaDestinoId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ParadaDestinoOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "ParadaDestino")]
    public string? ParadaDestinoLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaActividadViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
