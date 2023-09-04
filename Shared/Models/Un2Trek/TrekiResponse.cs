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
    public int Id { get; set; }

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Required]
    [StringLength(maximumLength: 50)]
    public string Title { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Description { get; set; } = string.Empty;
}
