using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ChecklistViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ChecklistViewModel_generated) ENABLED START*/
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
    public List<TareaItemViewModel> Tareas { get; set; } = new();

    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }
    /*PROTECTED REGION END*/
}

public class TareaItemViewModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public bool Completada { get; set; }
}