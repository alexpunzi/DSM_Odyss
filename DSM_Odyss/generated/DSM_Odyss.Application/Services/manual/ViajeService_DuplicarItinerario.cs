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
        IReadOnlyList<ParadaDestino> paradasOriginales =
            await _unitOfWork.ParadaDestinoRepository.PorViajeAsync(original.Id);
        IReadOnlyList<ParadaActividad> actividadesOriginales =
            await _unitOfWork.ParadaActividadRepository.ReadAllAsync();
        ParadaActividad[] actividadesDelViaje = actividadesOriginales
            .Where(actividad => actividad.Viaje.Id == original.Id)
            .ToArray();
        Viaje duplicado = new Viaje
        {
            Titulo = p_nuevoTitulo.Trim(),
            FechaInicio = original.FechaInicio,
            FechaFin = original.FechaFin,
            EsPublico = false,
            Presupuesto = original.Presupuesto,
            TipoViaje = original.TipoViaje,
            Estado = EstadoViaje.planificado,
        };

        // A duplicate is a new private plan: it keeps itinerary data, not collaborators or memories.
        await _repository.SaveAsync(duplicado);
        foreach (ParadaDestino paradaOriginal in paradasOriginales.OrderBy(parada => parada.Orden))
        {
            ParadaDestino paradaDuplicada = new()
            {
                Orden = paradaOriginal.Orden,
                FechaLlegada = paradaOriginal.FechaLlegada,
                FechaSalida = paradaOriginal.FechaSalida,
                Viaje = duplicado,
                Destino = paradaOriginal.Destino,
            };
            await _unitOfWork.ParadaDestinoRepository.SaveAsync(paradaDuplicada);

            foreach (ParadaActividad actividadOriginal in actividadesDelViaje
                         .Where(actividad => actividad.ParadaDestino?.Id == paradaOriginal.Id))
            {
                ParadaActividad actividadDuplicada = new()
                {
                    Nombre = actividadOriginal.Nombre,
                    Dia = actividadOriginal.Dia,
                    Horario = actividadOriginal.Horario,
                    Detalles = actividadOriginal.Detalles,
                    Completada = false,
                    Viaje = duplicado,
                    Ubicacion = actividadOriginal.Ubicacion,
                    ParadaDestino = paradaDuplicada,
                };
                await _unitOfWork.ParadaActividadRepository.SaveAsync(actividadDuplicada);
            }
        }

        foreach (ParadaActividad actividadOriginal in actividadesDelViaje
                     .Where(actividad => actividad.ParadaDestino is null))
        {
            await _unitOfWork.ParadaActividadRepository.SaveAsync(new ParadaActividad
            {
                Nombre = actividadOriginal.Nombre,
                Dia = actividadOriginal.Dia,
                Horario = actividadOriginal.Horario,
                Detalles = actividadOriginal.Detalles,
                Completada = false,
                Viaje = duplicado,
                Ubicacion = actividadOriginal.Ubicacion,
            });
        }

        return duplicado;
        /*PROTECTED REGION END*/
    }
}
