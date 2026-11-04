using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Estadisticas_generarPassportRecap) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class EstadisticasService
{
    public async Task<string> GenerarPassportRecapAsync(int p_oid, int p_anyo)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Estadisticas_generarPassportRecap) ENABLED START*/
        if (p_anyo < 1)
        {
            throw new ModelException("The recap year must be a positive number.");
        }

        Estadisticas estadisticas = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Estadisticas not found.");
        Cliente propietario = await ObtenerPropietarioAsync(estadisticas);
        Viaje[] viajes = propietario.Viajes
            .Where(viaje => viaje.FechaInicio.Year == p_anyo)
            .ToArray();

        Destino[] destinos = viajes
            .SelectMany(viaje => viaje.ParadasDestino)
            .Select(parada => parada.Destino)
            .Where(destino => destino is not null)
            .ToArray();
        string[] paises = destinos
            .Where(destino => !string.IsNullOrWhiteSpace(destino.Pais))
            .Select(destino => destino.Pais.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(pais => pais, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        int fotos = viajes
            .SelectMany(viaje => viaje.Carpetas?.Multimedia ?? Array.Empty<Multimedia>())
            .Count(multimedia => multimedia.Tipo == TipoMultimedia.foto);
        string? destinoMasVisitado = destinos
            .GroupBy(destino => string.IsNullOrWhiteSpace(destino.Ciudad)
                ? destino.Pais
                : destino.Ciudad,
                StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(grupo => grupo.Count())
            .ThenBy(grupo => grupo.Key, StringComparer.OrdinalIgnoreCase)
            .Select(grupo => grupo.Key)
            .FirstOrDefault();

        estadisticas.Anyo = p_anyo;
        estadisticas.PaisesVisitados = paises.Length;
        estadisticas.FotosSubidas = fotos;
        estadisticas.DestinoMasVisitado = destinoMasVisitado;
        await _repository.SaveAsync(estadisticas);

        // Kilometres are not included because the current domain model has no distance data.
        return $"Passport Recap {p_anyo}: {paises.Length} países visitados, {fotos} fotos subidas, destino más visitado: {destinoMasVisitado ?? "ninguno"}.";
        /*PROTECTED REGION END*/
    }
}
