using SycappsWeb.Shared.Entities.Un2Trek;
using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models.Un2Trek;

public class TrekiResponse
{
    public int Id { get; set; }
    public double Latitude { get; set; }

    public double Longitude { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;    
}
public class TrekiRequest
{
    [Range(-90, 90)]
    public double Latitud { get; set; }

    [Range(-180, 180)]
    public double Longitud { get; set; }

    [Required]
    [StringLength(maximumLength: 50)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Descripcion { get; set; } = string.Empty;
}
