using SycappsWeb.Shared.Entities;

namespace SycappsWeb.Shared.Models;

public class PuntoDto
{
    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string Distrito { get; set; } = string.Empty;

    public string Barrio { get; set; } = string.Empty;

    public string Calle { get; set; } = string.Empty;

    public string NumFinca { get; set; } = string.Empty;

    public string TipoReserva { get; set; } = string.Empty;

    public string FormaAparcamiento { get; set; } = string.Empty;

    public int NumPlazas { get; set; }

    public string Texto { get; set; } = string.Empty;

    public string UrlImagen { get; set; } = string.Empty;

    public double DistanciaAPosicionActual { get; set; }

    public static explicit operator PuntodeInteres(PuntoDto punto)
    {
        return new PuntodeInteres
        {
            Barrio = punto.Barrio,
            Calle = punto.Calle,
            Descripcion = punto.Descripcion,
            Distrito = punto.Distrito,
            FormaAparcamiento = punto.FormaAparcamiento,
            Latitud = punto.Latitud,
            Longitud = punto.Longitud,
            NumFinca = punto.NumFinca,
            NumPlazas = punto.NumPlazas,
            TipoReserva = punto.TipoReserva,
            Texto = punto.Texto,
            UrlImagen = punto.UrlImagen            
        };
    }
}
