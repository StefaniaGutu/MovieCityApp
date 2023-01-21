using AutoMapper;
using MovieCity.BusinessLogic.Implementation.FeedImp.Models;
using MovieCity.DataAccess;
using MovieCity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.FeedImp.Mappings
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<Post, ListPostsModel>()
                .ForMember(a => a.IsReview, a => a.MapFrom(src => src.ReviewId != null ? true : false))
                .ForMember(a => a.MovieId, a => a.MapFrom(src => src.Review.MovieSeriesId))
                .ForMember(a => a.MovieTitle, a => a.MapFrom(src => src.Review.MovieSeries.Title))
                .ForMember(a => a.MovieImage, a => a.MapFrom(src => src.Review.MovieSeries.MovieOrSeriesImage != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.Review.MovieSeries.MovieOrSeriesImage.Image)) : ""))
                .ForMember(a=> a.HasAvailableImage, a => a.MapFrom(src => src.Review.MovieSeries.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<Comment, CommentModel>();
        }
    }
}
