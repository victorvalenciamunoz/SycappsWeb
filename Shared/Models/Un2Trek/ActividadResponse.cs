using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models.Un2Trek;

public class IsStringValidDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;
            
            DateTime date = Convert.ToDateTime(value);
            return ValidationResult.Success;
        }
        catch (Exception)
        {
            return new ValidationResult("Cadena no válida como fecha");
        }
    }
}
public class ActividadResponse
{
    public ActividadResponse()
    {
            
    }
    public ActividadResponse(int id, string titulo, string validoDesde,string validoHasta)
    {
        Id = id;
        Titulo = titulo;
        ValidoDesde = validoDesde;
        ValidoHasta = validoHasta;
    }
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar un título")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage ="Debes indicar una fecha de comienzo")]
    [IsStringValidDateTime()]
    public string ValidoDesde { get; set; }

    [IsStringValidDateTime()]
    public string ValidoHasta { get; set; }
}
public record ActividadRequest
{
    public ActividadRequest(string titulo, DateTime validoDesde, DateTime? validoHasta)
    {
        Titulo = titulo;
        ValidoDesde = validoDesde;
        ValidoHasta = validoHasta;
    }
    public string Titulo { get; set; } = string.Empty;
    public DateTime ValidoDesde { get; set; }
    public DateTime? ValidoHasta { get; set; }
}
