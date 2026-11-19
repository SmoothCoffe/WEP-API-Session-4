namespace WebApplication1.Dtos.AuthorDtos
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }
}
