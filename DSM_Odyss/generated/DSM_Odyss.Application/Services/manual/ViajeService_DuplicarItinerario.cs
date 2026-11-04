using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Viaje_duplicarItinerario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ViajeService
{
    public async Task<Viaje> DuplicarItinerarioAsync(int p_oid, string p_nuevoTitulo)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Viaje_duplicarItinerario) ENABLED START*/
        if (string.IsNullOrWhiteSpace(p_nuevoTitulo))
        {
            throw new ModelException("A title is required for the duplicated itinerary.");
        }

        Viaje original = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Viaje not found.");
        Viaje duplicado = new Viaje
        {
            Titulo = p_nuevoTitulo.Trim(),
            FechaInicio = original.FechaInicio,
            FechaFin = original.FechaFin,
            EsPublico = false,
            Presupuesto = original.Presupuesto,
            TipoViaje = original.TipoViaje,
            Estado = EstadoViaje.planificado,
            Destino = original.Destino,
            Paradas = original.Paradas
                .Select(actividad => new ParadaActividad
                {
                    Nombre = actividad.Nombre,
                    Dia = actividad.Dia,
                    Horario = actividad.Horario,
                    Detalles = actividad.Detalles,
                    Completada = false,
                    Ubicacion = actividad.Ubicacion,
                })
                .ToList(),
        };

        // A duplicate is a new private plan: it keeps itinerary data, not collaborators or memories.
        await _repository.SaveAsync(duplicado);
        return duplicado;
        /*PROTECTED REGION END*/
    }
}
