using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Entities.Un2Trek;

public class Actividad
{
    public int Id { get; set; }

    [StringLength(maximumLength: 50)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(maximumLength: 150)]
    public string Descripcion { get; set; } = string.Empty;
    
    [Required]
    public DateTime ValidoDesde { get; set; }

    public DateTime? ValidoHasta { get; set; }

    public List<Treki> Treki { get; set; } = new();
}