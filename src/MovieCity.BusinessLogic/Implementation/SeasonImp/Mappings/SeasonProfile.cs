using AutoMapper;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp.Mappings
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<Season, EditSeasonModel>();

            CreateMap<Season, ListSeasonModel>();

            CreateMap<SeasonModel, Season>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.MovieSeriesId, a=> a.MapFrom(s => s.SeriesId));
        }
    }
}
