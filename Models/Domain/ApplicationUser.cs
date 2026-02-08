using Microsoft.AspNetCore.Identity;

namespace CineBookerEnterprise.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
