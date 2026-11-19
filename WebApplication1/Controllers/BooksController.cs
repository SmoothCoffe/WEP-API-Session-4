using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos.BookDtos;
using WebApplication1.Entities;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ActionExecutionLoggingFilter))]
    [ServiceFilter(typeof(ActionTimingFilter))]
    [ServiceFilter(typeof(ExceptionHandlingFilter))]
    public class BooksController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var books = await dbContext.Books
                .Include(book => book.Author)
                .OrderBy(book => book.Title)
                .ToListAsync();

            var response = mapper.Map<List<Book>, List<GetBookResponse>>(books);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await dbContext.Books
                .Include(book => book.Author)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (book is null)
                return NotFound(new { message = $"Book with Id = {id} is not found." });

            var response = mapper.Map<Book, GetBookResponse>(book);

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var authorExists = await dbContext.Authors.AnyAsync(author => author.Id == request.AuthorId);

            if (!authorExists)
                return BadRequest(new { message = $"Author with Id = {request.AuthorId} is not found." });

            var book = new Book
            {
                Title = request.Title,
                Price = request.Price,
                PublishedYear = request.PublishedYear,
                AuthorId = request.AuthorId
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, new { book.Id });
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateBookRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { message = "The route id does not match the body id." });

            var book = await dbContext.Books.FindAsync(id);

            if (book is null)
                return NotFound(new { message = $"Book with Id = {id} is not found." });

            var authorExists = await dbContext.Authors.AnyAsync(author => author.Id == request.AuthorId);

            if (!authorExists)
                return BadRequest(new { message = $"Author with Id = {request.AuthorId} is not found." });

            book.Title = request.Title;
            book.Price = request.Price;
            book.PublishedYear = request.PublishedYear;
            book.AuthorId = request.AuthorId;

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {
            if (id <= 0)
                return BadRequest();

            var book = await dbContext.Books.FindAsync(id);

            if (book is null)
                return NotFound(new { message = $"Book with Id = {id} is not found." });

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
