using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class RecordatorioViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(RecordatorioViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Detalles { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Hora { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.TipoAviso), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.TipoAviso TipoAviso { get; set; }

    public bool Enviado { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Tarea")]
    public int? TareaId { get; set; }
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> TareaOptions { get; set; } = new();
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Tarea")]
    public string? TareaLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(RecordatorioViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
