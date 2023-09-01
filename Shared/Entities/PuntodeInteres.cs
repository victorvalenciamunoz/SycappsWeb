using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;
using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Entities;

public class PuntodeInteres
{
    public int Id { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    [StringLength(maximumLength: 250)]
    public string Descripcion { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Distrito { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Barrio { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Calle { get; set; } = string.Empty;

    [StringLength(maximumLength: 20)]
    public string NumFinca { get; set; } = string.Empty;

    [StringLength(maximumLength: 50)]
    public string TipoReserva { get; set; } = string.Empty;

    [StringLength(maximumLength: 120)]
    public string FormaAparcamiento { get; set; } = string.Empty;

    public int NumPlazas { get; set; }

    public string Texto { get; set; } = string.Empty;

    public string UrlImagen { get; set; } = string.Empty;
    public static explicit operator PuntoDto(PuntodeInteres punto)
    {
        return new PuntoDto
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
