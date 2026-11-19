using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Entities;
using WebApplication1.Results;

namespace WebApplication1.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _dbContext.Authors.Include(a => a.Books).ToListAsync();

        public async Task<PaginatedList<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var query = _dbContext.Authors.Include(a => a.Books).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var authors = await query
                .OrderBy(a => a.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Author>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = authors
            };
        }

        public async Task<Author?> GetByIdAsync(int id)
            => await _dbContext.Authors
                              .Include(a => a.Books)
                              .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Author> CreateAsync(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return author;
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            var entity = await _dbContext.Authors.FindAsync(author.Id);

            if (entity is null)
                return false;

            entity.Name = author.Name;
            entity.Bio = author.Bio;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Authors.FindAsync(id);

            if (entity is null)
                return false;

            _dbContext.Authors.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
