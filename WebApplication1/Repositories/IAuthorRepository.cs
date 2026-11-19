using WebApplication1.Entities;
using WebApplication1.Results;

namespace WebApplication1.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<PaginatedList<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Author?> GetByIdAsync(int id);
        Task<Author> CreateAsync(Author author);
        Task<bool> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}
