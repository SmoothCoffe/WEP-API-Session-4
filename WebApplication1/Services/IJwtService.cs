using WebApplication1.Dtos.AccountDtos;
using WebApplication1.Entities;

namespace WebApplication1.Services
{
    public interface IJwtService
    {
        AuthResponse GenerateToken(ApplicationUser user);
    }
}
