namespace WebApplication1.Dtos.AuthorDtos
{
    public class UpdateAuthorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }
}
