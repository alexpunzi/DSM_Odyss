// Código manual (no generado por HARIA): destinos y actividades del viaje
// para la vista de detalle y el formulario de edición.
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Services;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.Controllers;

public partial class ViajeController
{
    // El ViajeAssembler no copia destinos ni actividades: lo hacemos aquí.
    private static void CargarDestinosYActividades(Viaje entity, ViajeViewModel model)
    {
        var paradas = entity.ParadasDestino.OrderBy(p => p.Orden).ToList();

        model.Destinos = paradas
            .Select(p => new DestinoParadaItemViewModel
            {
                DestinoId = p.Destino?.Id,
                Ciudad = p.Destino?.Ciudad,
                Pais = p.Destino?.Pais,
                Orden = p.Orden,
                FechaLlegada = p.FechaLlegada,
                FechaSalida = p.FechaSalida
            })
            .ToList();

        int? IndiceDestino(ParadaActividad actividad)
        {
            if (actividad.ParadaDestino is null) return null;
            var indice = paradas.FindIndex(p => p.Id == actividad.ParadaDestino.Id);
            return indice >= 0 ? indice : null;
        }

        model.Paradas = entity.Paradas
            .OrderBy(a => a.Dia)
            .ThenBy(a => a.Horario)
            .Select(a => new ParadaItemViewModel
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Dia = a.Dia,
                // Si la hora es 00:00 es que no se indicó horario.
                Horario = a.Horario.TimeOfDay == TimeSpan.Zero ? null : a.Horario.ToString("HH:mm"),
                Detalles = a.Detalles,
                Completada = a.Completada,
                DestinoIndex = IndiceDestino(a)
            })
            .ToList();
    }

    // El formulario necesita al menos una tarjeta de destino y una de actividad para pintarse bien.
    private static void AsegurarFilasMinimas(ViajeViewModel model)
    {
        if (!model.Destinos.Any())
            model.Destinos.Add(new DestinoParadaItemViewModel { Orden = 1, FechaLlegada = model.FechaInicio, FechaSalida = model.FechaFin });
        if (!model.Paradas.Any())
            model.Paradas.Add(new ParadaItemViewModel { Dia = 1 });
    }

    // Al editar: borra los destinos y actividades anteriores y crea los del formulario
    // (misma lógica que al crear el viaje). Se llama dentro de la transacción de Edit.
    private static async Task ReemplazarDestinosYActividadesAsync(IUnitOfWork unitOfWork, Viaje entity, ViajeViewModel model)
    {
        // 1. Borrar lo anterior (primero las actividades, que pueden apuntar a una parada).
        foreach (var actividad in entity.Paradas.ToList())
            await unitOfWork.ParadaActividadRepository.DeleteAsync(actividad);
        foreach (var parada in entity.ParadasDestino.ToList())
            await unitOfWork.ParadaDestinoRepository.DeleteAsync(parada);
        entity.Paradas.Clear();
        entity.ParadasDestino.Clear();

        var destinoService = new DestinoService(unitOfWork);
        var paradaDestinoService = new ParadaDestinoService(unitOfWork);
        var paradaActividadService = new ParadaActividadService(unitOfWork);

        // 2. Crear los destinos del formulario.
        var catalogoDestinos = await unitOfWork.DestinoRepository.ReadAllAsync();
        var paradaPorIndice = new Dictionary<int, ParadaDestino>(); // índice de la tarjeta -> parada creada
        int ordenActual = 1;
        for (int i = 0; i < model.Destinos.Count; i++)
        {
            var itemDestino = model.Destinos[i];
            if (string.IsNullOrWhiteSpace(itemDestino.Ciudad) && !itemDestino.DestinoId.HasValue)
                continue; // tarjeta vacía

            int destinoOid;
            if (itemDestino.DestinoId is int idCatalogo && idCatalogo > 0)
            {
                destinoOid = idCatalogo;
            }
            else
            {
                var ciudad = (itemDestino.Ciudad ?? string.Empty).Trim();
                var pais = string.IsNullOrWhiteSpace(itemDestino.Pais) ? "Sin especificar" : itemDestino.Pais.Trim();

                var existente = catalogoDestinos.FirstOrDefault(d =>
                    string.Equals(d.Ciudad, ciudad, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(d.Pais, pais, StringComparison.OrdinalIgnoreCase));

                if (existente is not null)
                {
                    destinoOid = existente.Id;
                }
                else
                {
                    var nuevo = await destinoService.CrearDestinoAsync(pais);
                    nuevo.Ciudad = ciudad;
                    await destinoService.ModifierDestinoDefaultAsync(nuevo);
                    destinoOid = nuevo.Id;
                }
            }

            paradaPorIndice[i] = await paradaDestinoService.CrearParadaDestinoAsync(
                ordenActual, itemDestino.FechaLlegada, itemDestino.FechaSalida, entity.Id, destinoOid);
            ordenActual++;
        }

        // 3. Crear las actividades del formulario.
        foreach (var act in model.Paradas.Where(p => !string.IsNullOrWhiteSpace(p.Nombre)))
        {
            var fecha = model.FechaInicio.Date.AddDays(Math.Max(0, act.Dia - 1));
            if (!string.IsNullOrWhiteSpace(act.Horario) && TimeSpan.TryParse(act.Horario, out var hora))
                fecha = fecha.Add(hora);

            var actividad = await paradaActividadService.CrearParadaActividadAsync(
                act.Nombre!.Trim(), act.Dia, fecha, false, entity.Id);

            // Detalles y destino de la actividad (el servicio de crear no los recibe).
            actividad.Detalles = string.IsNullOrWhiteSpace(act.Detalles) ? null : act.Detalles.Trim();
            if (act.DestinoIndex is int indice && paradaPorIndice.TryGetValue(indice, out var paradaDeLaActividad))
                actividad.ParadaDestino = paradaDeLaActividad;
            await unitOfWork.ParadaActividadRepository.SaveAsync(actividad);
        }
    }
}