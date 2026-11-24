using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSM_Odyss.Web.ViewModels;

public sealed class ViajeViewModel : IValidatableObject
{
    /*PROTECTED REGION ID(ViajeViewModel_generated) START*/
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    public string Titulo { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(3);

    public bool EsPublico { get; set; }

    public float Presupuesto { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.TipoViaje), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.TipoViaje TipoViaje { get; set; }

    [EnumDataType(typeof(DSM_Odyss.Domain.Enums.EstadoViaje), ErrorMessage = "El valor de {0} no es válido.")]
    public DSM_Odyss.Domain.Enums.EstadoViaje Estado { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Carpetas")]
    public int? CarpetasId { get; set; }
    [BindNever, ValidateNever]
    public List<SelectListItem> CarpetasOptions { get; set; } = new();
    [BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Carpetas")]
    public string? CarpetasLabel { get; set; }

    [UIHint("HariaAssociation")]
    [Display(Name = "Collages")]
    public int? CollagesId { get; set; }
    [BindNever, ValidateNever]
    public List<SelectListItem> CollagesOptions { get; set; } = new();
    [BindNever, UIHint("HariaAssociationLabel"), Display(Name = "Collages")]
    public string? CollagesLabel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in ValidateAdditional(validationContext)) yield return result;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(ViajeViewModel_manual) ENABLED START*/
    public string? DestinoNombre { get; set; }
    public List<ParadaItemViewModel> Paradas { get; set; } = new();

    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        if (FechaFin < FechaInicio)
        {
            yield return new ValidationResult("La fecha de fin no puede ser anterior a la de inicio.", new[] { nameof(FechaFin) });
        }
    }
    /*PROTECTED REGION END*/
}

public class ParadaItemViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Dia { get; set; } = 1;
    public string? Horario { get; set; }
    public string? Detalles { get; set; }
    public float Coste { get; set; }
}