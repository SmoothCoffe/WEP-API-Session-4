namespace WebApplication1.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
