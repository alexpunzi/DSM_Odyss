using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

        /*PROTECTED REGION ID(usingDSM_Odyss.Application.Services_Collage_generarCollageAutomatico) ENABLED START*/
        // references to other libraries
        /*PROTECTED REGION END*/

namespace DSM_Odyss.Application.Services;

public sealed partial class CollageService
{
    public async Task<Collage> GenerarCollageAutomaticoAsync(int p_viaje_oid)
    {
        /*PROTECTED REGION ID(DSM_Odyss.Application.Services_Collage_generarCollageAutomatico) ENABLED START*/
        Viaje viaje = await _unitOfWork.ViajeRepository.ReadAsync(p_viaje_oid)
            ?? throw new ModelException("Viaje not found.");
        Multimedia[] fotos = viaje.Carpetas?.Multimedia
            .Where(multimedia => multimedia.Tipo == TipoMultimedia.foto)
            .ToArray()
            ?? Array.Empty<Multimedia>();
        if (fotos.Length == 0)
        {
            throw new ModelException("The trip has no photos to use in a collage.");
        }

        int columnas = (int)Math.Ceiling(Math.Sqrt(fotos.Length));
        int filas = (int)Math.Ceiling(fotos.Length / (double)columnas);
        const int ancho = 1200;
        const int alto = 800;
        int anchoCelda = ancho / columnas;
        int altoCelda = alto / filas;
        // The model stores only a URL, so keep the generated SVG together with its image references as a data URL.
        var svg = new System.Text.StringBuilder(
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {ancho} {alto}\">");
        for (int indice = 0; indice < fotos.Length; indice++)
        {
            int x = indice % columnas * anchoCelda;
            int y = indice / columnas * altoCelda;
            string href = System.Security.SecurityElement.Escape(fotos[indice].Nombre) ?? string.Empty;
            svg.Append($"<image href=\"{href}\" x=\"{x}\" y=\"{y}\" width=\"{anchoCelda}\" height=\"{altoCelda}\" preserveAspectRatio=\"xMidYMid slice\"/>");
        }
        svg.Append("</svg>");

        Collage collage = new Collage
        {
            UrlImagen = $"data:image/svg+xml;base64,{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svg.ToString()))}",
            FechaCreacion = DateTime.UtcNow.Date,
        };
        await _repository.SaveAsync(collage);
        viaje.Collages = collage;
        await _unitOfWork.ViajeRepository.SaveAsync(viaje);
        return collage;
        /*PROTECTED REGION END*/
    }
}
