using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Services;

public sealed partial class EstadisticasService
{
    private async Task<Cliente> ObtenerPropietarioAsync(Estadisticas estadisticas)
    {
        IReadOnlyList<Cliente> clientes = await _unitOfWork.ClienteRepository.ReadAllAsync();
        return clientes.FirstOrDefault(cliente =>
                   cliente.Estadisticas.Any(item => item.Id == estadisticas.Id))
               ?? throw new ModelException("No client is associated with these statistics.");
    }
}
