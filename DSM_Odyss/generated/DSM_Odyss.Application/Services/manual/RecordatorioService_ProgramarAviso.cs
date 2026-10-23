using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Recordatorio_programarAviso) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class RecordatorioService
{
    public async Task ProgramarAvisoAsync(int p_oid, DateTime p_fecha, DateTime p_hora, TipoAviso p_tipo)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Recordatorio_programarAviso) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Recordatorio.programarAviso is pending.", new NotImplementedException("Method ProgramarAvisoAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
