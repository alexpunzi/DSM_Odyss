using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.UseCases_Checklist_vaciarChecklist) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.UseCases.Checklist;

public sealed partial class ChecklistUseCase
{
    public async Task VaciarChecklistAsync(int p_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.UseCases_Checklist_vaciarChecklist) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ChecklistService checklistService = null!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ChecklistUseCase.VaciarChecklistAsync");
            checklistService = new ChecklistService(_unitOfWork);

            await checklistService.VaciarChecklistAsync(p_oid);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Checklist.vaciarChecklist.", ex);
        }
        /*PROTECTED REGION END*/
    }
}
