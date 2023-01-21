using AutoMapper;
using MovieCity.BusinessLogic.Implementation.ReviewImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.ReviewImp.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewModel>()
                .ForMember(a => a.Username, a => a.MapFrom(src => src.User.Username))
                .ForMember(a => a.UserImage, a => a.MapFrom(src => src.User.UserImage != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.User.UserImage.Image)) : null));

            CreateMap<Review, ReviewListModel>()
                .ForMember(a => a.MovieTitle, a => a.MapFrom(src => src.MovieSeries.Title))
                .ForMember(a => a.MovieId, a => a.MapFrom(src => src.MovieSeries.Id))
                .ForMember(a => a.IsSeries, a => a.MapFrom(src => src.MovieSeries.IsSeries));
        } 
    }
}
