using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ParadaDestinoViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ParadaDestinoViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    public int Orden { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaLlegada { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaSalida { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [UIHint("HariaAssociation")]
    [Display(Name = "Viaje")]
    public int? ViajeId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ViajeOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Viaje")]
    public string? ViajeLabel { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [UIHint("HariaAssociation")]
    [Display(Name = "Destino")]
    public int? DestinoId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> DestinoOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Destino")]
    public string? DestinoLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ParadaDestinoViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
