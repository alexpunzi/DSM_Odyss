using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.UseCases_Estadisticas_recalcularEstadisticas) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.UseCases.Estadisticas;

public sealed partial class EstadisticasUseCase
{
    public async Task RecalcularEstadisticasAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.UseCases_Estadisticas_recalcularEstadisticas) ENABLED START*/
        try
        {
            await _unitOfWork.BeginTransactionAsync("EstadisticasUseCase.RecalcularEstadisticasAsync");

            global::DSM_Odyss.Domain.Entities.Estadisticas estadisticas = await _unitOfWork.EstadisticasRepository.ReadAsync(p_oid)
                ?? throw new ModelException("Estadisticas not found.");
            IReadOnlyList<Cliente> clientes = await _unitOfWork.ClienteRepository.ReadAllAsync();
            Cliente propietario = clientes.FirstOrDefault(cliente =>
                    cliente.Estadisticas.Any(item => item.Id == estadisticas.Id))
                ?? throw new ModelException("No client is associated with these statistics.");
            int anyo = estadisticas.Anyo ?? DateTime.UtcNow.Year;
            global::DSM_Odyss.Domain.Entities.Viaje[] viajes = propietario.Viajes
                .Where(viaje => viaje.FechaInicio.Year == anyo)
                .ToArray();

            estadisticas.Anyo = anyo;
            estadisticas.PaisesVisitados = viajes
                .Where(viaje => viaje.Destino is not null && !string.IsNullOrWhiteSpace(viaje.Destino.Pais))
                .Select(viaje => viaje.Destino.Pais)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();
            estadisticas.FotosSubidas = viajes
                .SelectMany(viaje => viaje.Carpetas?.Multimedia ?? Array.Empty<Multimedia>())
                .Count(multimedia => multimedia.Tipo == TipoMultimedia.foto);
            estadisticas.DestinoMasVisitado = viajes
                .Where(viaje => viaje.Destino is not null)
                .GroupBy(viaje => string.IsNullOrWhiteSpace(viaje.Destino.Ciudad)
                    ? viaje.Destino.Pais
                    : viaje.Destino.Ciudad,
                    StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(grupo => grupo.Count())
                .ThenBy(grupo => grupo.Key, StringComparer.OrdinalIgnoreCase)
                .Select(grupo => grupo.Key)
                .FirstOrDefault();

            await _unitOfWork.EstadisticasRepository.SaveAsync(estadisticas);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Estadisticas.recalcularEstadisticas.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
