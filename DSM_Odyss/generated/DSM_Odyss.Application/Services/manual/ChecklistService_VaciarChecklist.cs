using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Checklist_vaciarChecklist) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class ChecklistService
{
    public async Task VaciarChecklistAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Checklist_vaciarChecklist) ENABLED START*/
        Checklist checklist = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Checklist not found.");

        foreach (Tarea tarea in checklist.Tareas.ToArray())
        {
            await _unitOfWork.TareaRepository.DeleteAsync(tarea);
        }

        checklist.Tareas.Clear();
        await _repository.SaveAsync(checklist);
        /*PROTECTED REGION END*/
    }
}
