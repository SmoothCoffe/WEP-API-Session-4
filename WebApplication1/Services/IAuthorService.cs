using WebApplication1.Dtos.AuthorDtos;
using WebApplication1.Results;

namespace WebApplication1.Services
{
    public interface IAuthorService
    {
        Task<GenericResult<PaginatedList<GetAuthorResponse>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<GenericResult<GetAuthorResponse>> GetByIdAsync(int id);
        Task<GenericResult<GetAuthorResponse>> CreateAsync(CreateAuthorRequest request);
        Task<GenericResult<GetAuthorResponse>> UpdateAsync(UpdateAuthorRequest request);
        Task<Result> DeleteAsync(int id);
    }
}
