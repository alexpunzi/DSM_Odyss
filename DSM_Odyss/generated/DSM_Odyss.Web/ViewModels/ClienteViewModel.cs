using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? ConfirmPassword { get; set; }

    public int? DiaNacimiento { get; set; }
    public int? MesNacimiento { get; set; }
    public int? AnioNacimiento { get; set; }

    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        // Validación de requisitos de contraseña y confirmación (solo si se introduce contraseña)
        if (!string.IsNullOrEmpty(Password))
        {
            if (Password.Length < 8 || !Regex.IsMatch(Password, @"\d") || !Regex.IsMatch(Password, @"[\W_]"))
            {
                yield return new ValidationResult(
                    "La contraseña debe tener al menos 8 caracteres, 1 número y 1 carácter especial.",
                    new[] { nameof(Password) });
            }

            if (ConfirmPassword != null && Password != ConfirmPassword)
            {
                yield return new ValidationResult(
                    "Las contraseñas no coinciden.",
                    new[] { nameof(ConfirmPassword) });
            }
        }

        // Validación de los 3 desplegables de fecha de nacimiento
        if (DiaNacimiento.HasValue || MesNacimiento.HasValue || AnioNacimiento.HasValue)
        {
            if (!DiaNacimiento.HasValue || !MesNacimiento.HasValue || !AnioNacimiento.HasValue)
            {
                yield return new ValidationResult(
                    "Debes seleccionar día, mes y año de nacimiento.",
                    new[] { nameof(FechaNacimiento) });
            }
            else
            {
                bool fechaValida = false;
                try
                {
                    FechaNacimiento = new DateTime(AnioNacimiento.Value, MesNacimiento.Value, DiaNacimiento.Value);
                    fechaValida = true;
                }
                catch
                {
                    fechaValida = false;
                }

                if (!fechaValida)
                {
                    yield return new ValidationResult(
                        "La fecha de nacimiento seleccionada no es válida.",
                        new[] { nameof(FechaNacimiento) });
                }
            }
        }
    }
    /*PROTECTED REGION END*/
}