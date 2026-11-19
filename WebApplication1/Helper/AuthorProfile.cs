using AutoMapper;
using WebApplication1.Dtos.AuthorDtos;
using WebApplication1.Entities;

namespace WebApplication1.Helper
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, GetAuthorResponse>()
                .ForMember(dest => dest.BookCount,
                    opt => opt.MapFrom(src => src.Books.Count));

            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();
        }
    }
}
