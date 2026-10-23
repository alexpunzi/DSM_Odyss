using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Checklist_vaciarTareas) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ChecklistService
{
    public async Task VaciarTareasAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Checklist_vaciarTareas) ENABLED START*/
        // Write here your custom code...

        throw new ModelException("Application service generation for Checklist.vaciarTareas is pending.", new NotImplementedException("Method VaciarTareasAsync() not yet implemented."));
        /*PROTECTED REGION END*/
    }
}
