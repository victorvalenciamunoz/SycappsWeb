using Microsoft.AspNetCore.Identity;

namespace SycappsWeb.Shared.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool ReceivePromotionalEmails { get; set; }
    }
}
