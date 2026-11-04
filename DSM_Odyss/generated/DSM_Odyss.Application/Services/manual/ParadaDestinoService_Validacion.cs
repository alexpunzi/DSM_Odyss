using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Services;

public sealed partial class ParadaDestinoService
{
    private async Task ValidarParadaAsync(
        Viaje viaje,
        int orden,
        DateTime fechaLlegada,
        DateTime fechaSalida,
        int paradaDestinoId = 0)
    {
        if (orden < 1)
        {
            throw new ModelException("Destination stop order must be a positive number.");
        }

        if (fechaLlegada.Date > fechaSalida.Date
            || fechaLlegada.Date < viaje.FechaInicio.Date
            || fechaSalida.Date > viaje.FechaFin.Date)
        {
            throw new ModelException("Destination stop dates must fit within the trip dates.");
        }

        IReadOnlyList<ParadaDestino> paradas = await _repository.PorViajeAsync(viaje.Id);
        if (paradas.Any(parada => parada.Id != paradaDestinoId && parada.Orden == orden))
        {
            throw new ModelException("Destination stop order must be unique within a trip.");
        }
    }
}
