using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.UseCases_Viaje_duplicarItinerario) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.UseCases.Viaje;

public sealed partial class ViajeUseCase
{
    public async Task<global::DSM_Odyss.Domain.Entities.Viaje> DuplicarItinerarioAsync(int p_oid, string p_nuevoTitulo)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.UseCases_Viaje_duplicarItinerario) ENABLED START*/
        // Resolve external infrastructure services with _utils!.GetRequiredService<TService>() when this use case receives IUnitOfWorkUtils.
        ViajeService viajeService = null!;
        global::DSM_Odyss.Domain.Entities.Viaje result = default(global::DSM_Odyss.Domain.Entities.Viaje)!;

        try
        {
            await _unitOfWork.BeginTransactionAsync("ViajeUseCase.DuplicarItinerarioAsync");
            viajeService = new ViajeService(_unitOfWork);

            result = await viajeService.DuplicarItinerarioAsync(p_oid, p_nuevoTitulo);

            // La copia pertenece al mismo autor que el viaje original (dentro de la misma transacción).
            IReadOnlyList<Cliente> clientes = await _unitOfWork.ClienteRepository.ReadAllAsync();
            Cliente? autor = clientes.FirstOrDefault(cliente => cliente.Viajes.Any(viaje => viaje.Id == p_oid));
            if (autor is not null)
            {
                autor.Viajes.Add(result);
                await _unitOfWork.ClienteRepository.SaveAsync(autor);
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new ModelException("Application error while executing use case Viaje.duplicarItinerario.", ex);
        }

        return result;
        /*PROTECTED REGION END*/
    }
}
