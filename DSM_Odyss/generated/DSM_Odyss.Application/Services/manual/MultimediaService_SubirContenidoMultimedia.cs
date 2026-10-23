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
        // Write here your custom code...

        throw new ModelException("Application service generation for Multimedia.subirContenidoMultimedia is pending.", new NotImplementedException("Method SubirContenidoMultimediaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
