namespace WebApplication1.Dtos.AccountDtos
{
    public record AuthResponse(string AccessToken, DateTime ExpiresAtUtc);
}
