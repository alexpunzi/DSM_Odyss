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
    // Lista de destinos del viaje (1 o más)
    public List<DestinoParadaItemViewModel> Destinos { get; set; } = new();

    // Lista de paradas del viaje (1 o más)
    public List<ParadaItemViewModel> Paradas { get; set; } = new();

    // Catálogo de destinos existentes para sugerir/seleccionar en la vista
    [BindNever, ValidateNever]
    public List<SelectListItem> DestinosCatalogoOptions { get; set; } = new();

    private IEnumerable<ValidationResult> ValidateAdditional(ValidationContext validationContext)
    {
        if (FechaFin < FechaInicio)
        {
            yield return new ValidationResult("La fecha de fin no puede ser anterior a la de inicio.",
            new[] { nameof(FechaFin) });
        }

        // Validar que haya al menos un destino
        var destinosValidos = Destinos.Where(d => !string.IsNullOrWhiteSpace(d.Ciudad) || d.DestinoId.HasValue).ToList();
        if (!destinosValidos.Any())
        {
            yield return new ValidationResult("Debes indicar al menos un destino.",
            new[] { nameof(Destinos) });
        }

        // Validar reglas de negocio de ParadaDestino del backend (fechas dentro del viaje y llegada <= salida)
        for (int i = 0; i < Destinos.Count; i++)
        {
            var d = Destinos[i];
            if (string.IsNullOrWhiteSpace(d.Ciudad) && !d.DestinoId.HasValue) continue;

            if (d.FechaSalida < d.FechaLlegada)
            {
                yield return new ValidationResult(
                    $"La fecha de salida del destino '{d.Ciudad}' no puede ser anterior a la fecha de llegada.",
                    new[] { $"{nameof(Destinos)}[{i}].{nameof(d.FechaSalida)}" }
                );
            }

            if (d.FechaLlegada < FechaInicio || d.FechaSalida > FechaFin)
            {
                yield return new ValidationResult(
                    $"Las fechas del destino '{d.Ciudad}' deben estar dentro del rango del viaje.",
                    new[] { $"{nameof(Destinos)}[{i}].{nameof(d.FechaLlegada)}", $"{nameof(Destinos)}[{i}].{nameof(d.FechaSalida)}" }
                );
            }
        }
    }
    /*PROTECTED REGION END*/
}

public class DestinoParadaItemViewModel
{
    public int? DestinoId { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public int Orden { get; set; } = 1;

    [DataType(DataType.Date)]
    public DateTime FechaLlegada { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime FechaSalida { get; set; } = DateTime.Today.AddDays(3);
}

public class ParadaItemViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Dia { get; set; } = 1;
    public string? Horario { get; set; }
    public string? Detalles { get; set; }
    public float Coste { get; set; }

    // Índice (0, 1, 2...) del destino al que pertenece la parada
    public int? DestinoIndex { get; set; }
}