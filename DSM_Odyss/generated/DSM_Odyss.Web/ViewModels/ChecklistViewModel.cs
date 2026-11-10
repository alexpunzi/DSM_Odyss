using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ChecklistViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ChecklistViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Nombre { get; set; } = string.Empty;



    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ChecklistViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
