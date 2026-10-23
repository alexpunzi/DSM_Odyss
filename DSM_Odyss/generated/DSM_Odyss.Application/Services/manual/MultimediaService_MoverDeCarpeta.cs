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
        // Write here your custom code...

        throw new ModelException("Application service generation for Multimedia.moverDeCarpeta is pending.", new NotImplementedException("Method MoverDeCarpetaAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
