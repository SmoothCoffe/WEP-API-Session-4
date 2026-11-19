using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
