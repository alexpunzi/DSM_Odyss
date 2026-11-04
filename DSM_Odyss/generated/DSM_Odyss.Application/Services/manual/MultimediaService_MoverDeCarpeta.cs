using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Multimedia_moverDeCarpeta) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class MultimediaService
{
    public async Task MoverDeCarpetaAsync(int p_oid, int p_nuevaCarpeta_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Multimedia_moverDeCarpeta) ENABLED START*/
        Multimedia multimedia = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Multimedia not found.");
        Carpeta nuevaCarpeta = await _unitOfWork.CarpetaRepository.ReadAsync(p_nuevaCarpeta_oid)
            ?? throw new ModelException("Target folder not found.");

        IReadOnlyList<Carpeta> carpetas = await _unitOfWork.CarpetaRepository.ReadAllAsync();
        Carpeta? carpetaActual = carpetas.FirstOrDefault(item =>
            item.Multimedia.Any(element => element.Id == multimedia.Id));
        if (carpetaActual is null)
        {
            throw new ModelException("The multimedia item is not assigned to a folder.");
        }

        if (carpetaActual.Id == nuevaCarpeta.Id)
        {
            return;
        }

        carpetaActual.Multimedia.Remove(multimedia);
        nuevaCarpeta.Multimedia.Add(multimedia);
        await _unitOfWork.CarpetaRepository.SaveAsync(carpetaActual);
        await _unitOfWork.CarpetaRepository.SaveAsync(nuevaCarpeta);
        /*PROTECTED REGION END*/
    }
}
