namespace WebApplication1.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
