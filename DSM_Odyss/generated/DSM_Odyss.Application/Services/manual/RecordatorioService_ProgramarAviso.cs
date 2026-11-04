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
        if (!Enum.IsDefined(p_tipo))
        {
            throw new ModelException("The reminder type is not supported.");
        }

        Recordatorio recordatorio = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Recordatorio not found.");
        recordatorio.Fecha = p_fecha.Date;
        recordatorio.Hora = p_hora;
        recordatorio.TipoAviso = p_tipo;
        recordatorio.Enviado = false;
        await _repository.SaveAsync(recordatorio);
        /*PROTECTED REGION END*/
    }
}
