using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Server.Models.Un2Trek;

public class TrekiDto
{
    public int Id { get; set; }
    public double Latitud { get; set; }

    public double Longitud { get; set; }

    [StringLength(maximumLength: 50)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Descripcion { get; set; } = string.Empty;
}