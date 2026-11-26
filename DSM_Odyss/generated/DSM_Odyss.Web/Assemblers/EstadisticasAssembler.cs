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
        Cliente cliente,
        IReadOnlyList<string> paisesColoreados,
        int? anyoFiltro = null)
    {
        var vm = ToViewModel(entity);
        vm.NombreUsuario = string.IsNullOrWhiteSpace(cliente.User) ? cliente.Email : cliente.User;
        vm.PaisesColoreados = paisesColoreados.ToList();

        var viajesCliente = cliente.Viajes ?? new List<Viaje>();
        vm.AnyosDisponibles = viajesCliente
            .Select(v => v.FechaInicio.Year)
            .Append(DateTime.Now.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToList();

        var viajesFiltrados = anyoFiltro.HasValue
            ? viajesCliente.Where(v => v.FechaInicio.Year == anyoFiltro.Value).ToList()
            : viajesCliente.ToList();

        vm.TotalViajes = viajesFiltrados.Count;

        // Archivos almacenados (fotos y vídeos en carpetas de viaje)
        vm.ArchivosAlmacenados = viajesFiltrados
            .SelectMany(v => v.Carpetas?.Multimedia ?? new List<Multimedia>())
            .Count();

        // Top destinos más visitados
        vm.TopDestinos = viajesFiltrados
            .SelectMany(v => v.ParadasDestino ?? new List<ParadaDestino>())
            .Where(pd => pd.Destino != null)
            .Select(pd => string.IsNullOrWhiteSpace(pd.Destino.Ciudad) ? pd.Destino.Pais : $"{pd.Destino.Ciudad} ({pd.Destino.Pais})")
            .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
            .GroupBy(nombre => nombre, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(3)
            .Select(g => g.Key)
            .ToList();

        // Puntuación media en reseñas
        var resenas = viajesFiltrados
            .SelectMany(v => v.Resenas ?? new List<Resena>())
            .Union(cliente.Resenas ?? new List<Resena>())
            .ToList();
        vm.PuntuacionMediaResenas = resenas.Count > 0
            ? Math.Round(resenas.Average(r => r.Puntuacion), 1)
            : 0.0;

        // Cálculo de kilómetros recorridos usando Latitud/Longitud de Ubicaciones en cada viaje
        double totalKm = 0;
        foreach (var viaje in viajesFiltrados)
        {
            var coordenadas = (viaje.Paradas ?? new List<ParadaActividad>())
                .Where(p => p.Ubicacion != null && (p.Ubicacion.Latitud != 0 || p.Ubicacion.Longitud != 0))
                .Select(p => p.Ubicacion!)
                .ToList();

            for (int i = 0; i < coordenadas.Count - 1; i++)
            {
                totalKm += CalcularDistanciaHaversineKm(
                    coordenadas[i].Latitud, coordenadas[i].Longitud,
                    coordenadas[i + 1].Latitud, coordenadas[i + 1].Longitud);
            }
        }
        vm.KilometrosRecorridos = Math.Round(totalKm, 1);

        return vm;
    }

    private static double CalcularDistanciaHaversineKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0; // Radio medio de la Tierra en km
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
    /*PROTECTED REGION END*/
}