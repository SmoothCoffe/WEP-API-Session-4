using AutoMapper;
using WebApplication1.Dtos.BookDtos;
using WebApplication1.Entities;

namespace WebApplication1.Helper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, GetBookResponse>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}
