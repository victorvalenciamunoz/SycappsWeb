using System.ComponentModel.DataAnnotations;

namespace SycappsWeb.Shared.Models.Un2Trek;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}
