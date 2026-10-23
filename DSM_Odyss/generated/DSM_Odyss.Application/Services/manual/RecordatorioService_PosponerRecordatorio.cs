using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Recordatorio_posponerRecordatorio) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class RecordatorioService
{
    public async Task PosponerRecordatorioAsync(int p_oid, DateTime p_nuevaFecha, DateTime p_nuevaHora)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Recordatorio_posponerRecordatorio) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Recordatorio.posponerRecordatorio is pending.", new NotImplementedException("Method PosponerRecordatorioAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
