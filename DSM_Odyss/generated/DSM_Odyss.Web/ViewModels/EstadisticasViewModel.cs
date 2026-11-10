using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class EstadisticasViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(EstadisticasViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [UIHint("HariaReadOnly:Create,Edit")]
    public int? Anyo { get; set; }

    [UIHint("HariaReadOnly:Create,Edit")]
    public int? PaisesVisitados { get; set; }

    [UIHint("HariaReadOnly:Create,Edit")]
    public int? FotosSubidas { get; set; }

    [UIHint("HariaReadOnly:Create,Edit")]
    public string? DestinoMasVisitado { get; set; }



    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(EstadisticasViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
