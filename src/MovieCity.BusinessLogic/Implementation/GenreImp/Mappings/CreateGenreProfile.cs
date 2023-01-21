using AutoMapper;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.GenreImp.Mappings
{
    public class CreateGenreProfile : Profile
    {
        public CreateGenreProfile()
        {
            CreateMap<CreateGenreModel, Genre>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));

            CreateMap<Genre, ListGenresModel>();

            CreateMap<Genre, EditGenreModel>();
        }
    }
}
