using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ClienteViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ClienteViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string User { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    [UIHint("HariaPassword")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.CanalNotificacion), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.CanalNotificacion CanalNotificacion { get; set; }

    public bool RecibirRecordatorios { get; set; }



    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ClienteViewModel_manual) ENABLED START*/
    // Add custom properties and validation here.
    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        yield break;
    }

    /*PROTECTED REGION END*/
}
