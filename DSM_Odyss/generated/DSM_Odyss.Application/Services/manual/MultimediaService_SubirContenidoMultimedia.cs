using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Multimedia_subirContenidoMultimedia) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class MultimediaService
{
    public async Task<Multimedia> SubirContenidoMultimediaAsync(int p_carpeta_oid, TipoMultimedia p_tipo, string p_nombre)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Multimedia_subirContenidoMultimedia) ENABLED START*/
        if (string.IsNullOrWhiteSpace(p_nombre))
        {
            throw new ModelException("A multimedia file name is required.");
        }

        if (!Enum.IsDefined(p_tipo))
        {
            throw new ModelException("The multimedia type is not supported.");
        }

        Carpeta carpeta = await _unitOfWork.CarpetaRepository.ReadAsync(p_carpeta_oid)
            ?? throw new ModelException("Carpeta not found.");
        Multimedia multimedia = new Multimedia
        {
            Tipo = p_tipo,
            Nombre = p_nombre.Trim(),
            FechaSubida = DateTime.UtcNow.Date,
            Descripcion = string.Empty,
        };

        carpeta.Multimedia.Add(multimedia);
        await _repository.SaveAsync(multimedia);
        await _unitOfWork.CarpetaRepository.SaveAsync(carpeta);
        return multimedia;
        /*PROTECTED REGION END*/
    }
}
