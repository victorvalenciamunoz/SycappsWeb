namespace SycappsWeb.Shared.Models.Un2Trek;

public class CaptureTrekiRequest
{
    public double TrekiLatitude { get; set; }
    public double TrekiLongitude { get; set; }
    public double CurrentLatitude { get; set; }
    public double CurrentLongitude { get; set; }
    public int ActivityId { get; set; }
}