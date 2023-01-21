using AutoMapper;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Mappings
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<CreateActorModel, Actor>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));

            CreateMap<Actor, EditActorModel>()
                .ForMember(a => a.ActualImage, a => a.MapFrom(src => src.ActorImage.Image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.ActorImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.ActorImage.Image != null ? true : false));

            CreateMap<Actor, ViewActorDetailsModel>()
                .ForMember(a => a.Image, a => a.MapFrom(src => src.ActorImage.Image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.ActorImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.ActorImage.Image != null ? true : false));

            CreateMap<Actor, ListActorWithImageModel>()
                .ForMember(a => a.Image, a => a.MapFrom(src => src.ActorImage.Image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.ActorImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.ActorImage.Image != null ? true : false));

            CreateMap<Actor, ListActorsModel>();
        }
    }
}
