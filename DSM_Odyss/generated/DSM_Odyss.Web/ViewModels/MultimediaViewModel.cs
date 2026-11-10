using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class MultimediaViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(MultimediaViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.TipoMultimedia), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.TipoMultimedia Tipo { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Nombre { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime FechaSubida { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Descripcion { get; set; } = string.Empty;



    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(MultimediaViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
