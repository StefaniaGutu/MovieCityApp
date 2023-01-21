using AutoMapper;
using MovieCity.BusinessLogic.Implementation.EpisodeImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.EpisodeImp.Mappings
{
    public class EpisodeProfile : Profile
    {
        public EpisodeProfile()
        {
            CreateMap<EpisodeModel, Episode>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));

            CreateMap<Episode, EpisodeModel>();

            CreateMap<Episode, ListEpisodeModel>();
        }
    }
}
