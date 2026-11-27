// HARIA-WEB MVC managed assembler
using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Web.Assemblers;

public static class EstadisticasAssembler
{
    /*PROTECTED REGION ID(EstadisticasAssembler_method_ToViewModel) START*/
    public static DSM_Odyss.Web.ViewModels.EstadisticasViewModel ToViewModel(Estadisticas entity) =>
        new()
        {
            Id = entity.Id,
            Anyo = entity.Anyo,
            PaisesVisitados = entity.PaisesVisitados,
            FotosSubidas = entity.FotosSubidas,
            DestinoMasVisitado = entity.DestinoMasVisitado,
        };
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(EstadisticasAssembler_method_ApplyToEntity) START*/
    public static void ApplyToEntity(DSM_Odyss.Web.ViewModels.EstadisticasViewModel model, Estadisticas entity)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);
        entity.Anyo = model.Anyo;
        entity.PaisesVisitados = model.PaisesVisitados;
        entity.FotosSubidas = model.FotosSubidas;
        entity.DestinoMasVisitado = model.DestinoMasVisitado;
    }
    /*PROTECTED REGION END*/

    /*PROTECTED REGION ID(EstadisticasAssembler_custom) ENABLED START*/
    public static DSM_Odyss.Web.ViewModels.EstadisticasViewModel ToDashboardViewModel(
        Estadisticas entity,
        IReadOnlyList<string> paisesColoreados,
        DSM_Odyss.Web.Controllers.EstadisticasController.EstadisticasMetricasCalculadas metricas)
    {
        var vm = ToViewModel(entity);
        vm.NombreUsuario = metricas.NombreUsuario;
        vm.PaisesColoreados = paisesColoreados.ToList();
        vm.AnyosDisponibles = metricas.AnyosDisponibles.ToList();
        vm.TotalViajes = metricas.TotalViajes;
        vm.ArchivosAlmacenados = metricas.ArchivosAlmacenados;
        vm.TopDestinos = metricas.TopDestinos.ToList();
        vm.PuntuacionMediaResenas = metricas.PuntuacionMediaResenas;
        return vm;
    }
    /*PROTECTED REGION END*/
}