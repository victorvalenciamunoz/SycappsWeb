using SycappsWeb.Shared.Models.Un2Trek;
using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Entities.Un2Trek;

public class Treki
{
    public int Id { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    [StringLength(maximumLength: 50)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(maximumLength: 250)]
    public string Descripcion { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public List<Actividad> Actividades { get; } = new();
}
