using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models.Un2Trek;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;


    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}
