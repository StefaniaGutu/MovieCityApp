using AutoMapper;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.Common.DTOs;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.Account.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ViewUserProfileModel>()
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.UserImage.Image != null ? true : false)); ;

            CreateMap<User, ViewUserItem>();

            CreateMap<User, UserModel>()
                .ForMember(a => a.Image, a => a.MapFrom(src => src.UserImage != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.UserImage.Image)) : null));
        }
    }
}
