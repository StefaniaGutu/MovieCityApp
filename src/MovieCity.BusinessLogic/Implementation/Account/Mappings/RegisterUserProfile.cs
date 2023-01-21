using AutoMapper;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.Account.Mappings
{
    public class RegisterUserProfile : Profile
    {
        public RegisterUserProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()));
        }
    }
}
