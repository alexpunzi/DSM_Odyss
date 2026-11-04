using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Estadisticas_obtenerPaisesColoreados) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class EstadisticasService
{
    public async Task<IReadOnlyList<string>> ObtenerPaisesColoreadosAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Estadisticas_obtenerPaisesColoreados) ENABLED START*/
        Estadisticas estadisticas = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Estadisticas not found.");
        Cliente propietario = await ObtenerPropietarioAsync(estadisticas);

        return propietario.Viajes
            .Where(viaje => viaje.Destino is not null && !string.IsNullOrWhiteSpace(viaje.Destino.Pais))
            .Select(viaje => viaje.Destino.Pais.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(pais => pais, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        /*PROTECTED REGION END*/
    }
}
