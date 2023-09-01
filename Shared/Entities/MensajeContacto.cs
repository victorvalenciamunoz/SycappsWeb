using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Entities;

public class MensajeContacto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Mensaje { get; set; }

    public bool Leido { get; set; }

    public DateTime FechaRecepcion { get; set; }

    public DateTime FechaLeido { get; set; }
}
