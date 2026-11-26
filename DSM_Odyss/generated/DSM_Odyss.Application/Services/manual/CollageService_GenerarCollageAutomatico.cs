using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Services;

public sealed partial class CollageService
{
    public async Task<Collage> GenerarCollageAutomaticoAsync(int p_viaje_oid, string imagenDataUrl)
    {
        Viaje viaje = await _unitOfWork.ViajeRepository.ReadAsync(p_viaje_oid)
            ?? throw new ModelException("Viaje not found.");

        Collage? collage = viaje.Collages;
        if (collage is not null)
        {
            var otherTrips = await _unitOfWork.ViajeRepository.ReadAllAsync();
            if (otherTrips.Any(other => other.Id != viaje.Id && other.Collages?.Id == collage.Id))
                collage = null;
        }

        collage ??= new Collage();
        collage.UrlImagen = imagenDataUrl;
        collage.FechaCreacion = DateTime.UtcNow.Date;
        await _repository.SaveAsync(collage);
        viaje.Collages = collage;
        await _unitOfWork.ViajeRepository.SaveAsync(viaje);
        return collage;
    }
}
