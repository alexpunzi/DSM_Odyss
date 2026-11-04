using DSM_Odyss.Application.Abstractions;

namespace DSM_Odyss.Application.UseCases.Checklist;

public sealed partial class ChecklistUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUnitOfWorkUtils? _utils;

    public ChecklistUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ChecklistUseCase(IUnitOfWork unitOfWork, IUnitOfWorkUtils utils)
        : this(unitOfWork)
    {
        _utils = utils;
    }
}
