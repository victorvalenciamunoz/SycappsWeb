using Microsoft.AspNetCore.Identity;

namespace SycappsWeb.Shared.Entities.Un2Trek;

public class CapturaTreki
{
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    public ApplicationUser Usuario { get; set; }
    public int TrekiId { get; set; }
    public Treki Treki { get; set; }
    public int ActividadId { get; set; }
    public Actividad Actividad { get; set; }
    public DateTime FechaCaptura { get; set; }
}
