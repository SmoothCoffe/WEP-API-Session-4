using AutoMapper;
using WebApplication1.Dtos.AuthorDtos;
using WebApplication1.Entities;
using WebApplication1.Repositories;
using WebApplication1.Results;

namespace WebApplication1.Services
{
    public class AuthorService(IAuthorRepository repository, IMapper mapper) : IAuthorService
    {
        public async Task<GenericResult<PaginatedList<GetAuthorResponse>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var authors = await repository.GetAllAsync(pageNumber, pageSize, searchTerm);

            var response = new PaginatedList<GetAuthorResponse>
            {
                PageNumber = authors.PageNumber,
                PageSize = authors.PageSize,
                TotalRecords = authors.TotalRecords,
                TotalPages = authors.TotalPages,
                Data = mapper.Map<IEnumerable<Author>, IEnumerable<GetAuthorResponse>>(authors.Data ?? [])
            };

            return GenericResult<PaginatedList<GetAuthorResponse>>.Success(response);
        }

        public async Task<GenericResult<GetAuthorResponse>> GetByIdAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity is null)
                return GenericResult<GetAuthorResponse>.Failure($"Author with Id = {id} is not found.");

            var response = mapper.Map<GetAuthorResponse>(entity);
            return GenericResult<GetAuthorResponse>.Success(response);
        }

        public async Task<GenericResult<GetAuthorResponse>> CreateAsync(CreateAuthorRequest request)
        {
            var author = mapper.Map<Author>(request);
            var created = await repository.CreateAsync(author);

            var response = mapper.Map<GetAuthorResponse>(created);
            return GenericResult<GetAuthorResponse>.Success(response);
        }

        public async Task<GenericResult<GetAuthorResponse>> UpdateAsync(UpdateAuthorRequest request)
        {
            var author = mapper.Map<Author>(request);

            if (!await repository.UpdateAsync(author))
                return GenericResult<GetAuthorResponse>.Failure($"Author with Id = {request.Id} is not found.");

            var updated = await repository.GetByIdAsync(request.Id);
            var response = mapper.Map<GetAuthorResponse>(updated!);
            return GenericResult<GetAuthorResponse>.Success(response);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var deleted = await repository.DeleteAsync(id);

            return deleted
                ? Result.Success()
                : Result.Failure($"Author with Id = {id} is not found.");
        }
    }
}
