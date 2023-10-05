using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models.Un2Trek;

public class CaptureTrekiRequest
{
    [Range(-90, 90)]
    public double TrekiLatitude { get; set; }

    [Range(-180, 180)]
    public double TrekiLongitude { get; set; }

    [Range(-90, 90)]
    public double CurrentLatitude { get; set; }

    [Range(-180, 180)]
    public double CurrentLongitude { get; set; }

    [Required]
    public int ActivityId { get; set; }
}