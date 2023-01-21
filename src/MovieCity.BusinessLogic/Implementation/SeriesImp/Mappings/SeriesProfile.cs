using AutoMapper;
using MovieCity.BusinessLogic.Implementation.SeriesImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Mappings
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<MoviesAndSeries, ViewSeriesDetailsModel>()
                .ForMember(a => a.Actors, a => a.MapFrom(src => src.Actors.Select(m => m.Name).ToList()))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Name).ToList()))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<MoviesAndSeries, EditSeriesModel>()
                .ForMember(a => a.Actors, a => a.MapFrom(src => src.Actors.Select(m => m.Id).ToList()))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Id).ToList()))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<MoviesAndSeries, SeriesDetailsModel>()
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Name).ToList()))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<CreateSeriesModel, MoviesAndSeries>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.Genres, a => a.Ignore())
                .ForMember(a => a.Actors, a => a.Ignore());
        }
    }
}
