using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Ubicacion_consultarUbicacionEnMapa) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class UbicacionService
{
    public async Task<string> ConsultarUbicacionEnMapaAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Ubicacion_consultarUbicacionEnMapa) ENABLED START*/
        Ubicacion ubicacion = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Ubicacion not found.");
        if (ubicacion.Latitud is < -90 or > 90 || ubicacion.Longitud is < -180 or > 180)
        {
            throw new ModelException("The location has invalid latitude or longitude coordinates.");
        }

        string latitud = ubicacion.Latitud.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
        string longitud = ubicacion.Longitud.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
        return $"https://www.google.com/maps/search/?api=1&query={latitud}%2C{longitud}";
        /*PROTECTED REGION END*/
    }
}
