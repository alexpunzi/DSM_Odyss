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
            throw new ModelException("El orden de los destinos debe ser un número positivo.");
        }

        if (fechaLlegada.Date > fechaSalida.Date
            || fechaLlegada.Date < viaje.FechaInicio.Date
            || fechaSalida.Date > viaje.FechaFin.Date)
        {
            throw new ModelException("Las fechas de cada destino deben estar dentro de las fechas del viaje (y la llegada no puede ser posterior a la salida).");
        }

        IReadOnlyList<ParadaDestino> paradas = await _repository.PorViajeAsync(viaje.Id);
        if (paradas.Any(parada => parada.Id != paradaDestinoId && parada.Orden == orden))
        {
            throw new ModelException("Dos destinos del mismo viaje no pueden tener el mismo orden.");
        }
    }
}
