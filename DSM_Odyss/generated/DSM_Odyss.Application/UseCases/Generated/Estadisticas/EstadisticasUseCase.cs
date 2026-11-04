using DSM_Odyss.Application.Abstractions;

namespace DSM_Odyss.Application.UseCases.Estadisticas;

public sealed partial class EstadisticasUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUnitOfWorkUtils? _utils;

    public EstadisticasUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public EstadisticasUseCase(IUnitOfWork unitOfWork, IUnitOfWorkUtils utils)
        : this(unitOfWork)
    {
        _utils = utils;
    }
}
