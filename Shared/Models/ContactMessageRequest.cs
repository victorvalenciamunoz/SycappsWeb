using SycappsWeb.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models;

public class ContactMessageRequest
{
    [Required(ErrorMessage = "Debes indicar un nombre")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Debes indicar una dirección de correo")]
    [EmailAddress(ErrorMessage = "Debes indicar una dirección de correo válida")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Debes rellenar el mensaje que nos quieres comunicar")]
    public string Mensaje { get; set; }

    public bool Leido { get; set; }

    public DateTime FechaRecepcion { get; set; }

    public DateTime FechaLeido { get; set; }

    public static explicit operator MensajeContacto(ContactMessageRequest message)
    {
        return new MensajeContacto
        {
            Email = message.Email,
            Nombre = message.Nombre,
            FechaLeido = message.FechaLeido,
            FechaRecepcion = message.FechaRecepcion,
            Leido = message.Leido,
            Mensaje = message.Mensaje
        };
    }
}
