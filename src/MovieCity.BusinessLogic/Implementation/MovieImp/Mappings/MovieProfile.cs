using AutoMapper;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, EditMovieModel>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.Actors, a => a.MapFrom(src => src.IdNavigation.Actors.Select(m => m.Id).ToList()));

            CreateMap<MoviesAndSeries, EditMovieModel>()
                .ForMember(a => a.Duration, a => a.MapFrom(src => src.Movie.Duration))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(a => a.Actors, a => a.MapFrom(src => src.Actors.Select(m => m.Id).ToList()))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Id).ToList()))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<MoviesAndSeries, MovieDetailsModel>()
                .ForMember(a => a.Duration, a => a.MapFrom(src => src.Movie.Duration))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Name).ToList()));

            CreateMap<MoviesAndSeries, ViewMovieDetailsModel>()
                .ForMember(a => a.Duration, a => a.MapFrom(src => src.Movie.Duration))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(a => a.Actors, a => a.MapFrom(src => src.Actors.Select(m => m.Name).ToList()))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.Genres.Select(m => m.Name).ToList()))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<MoviesAndSeries, ListMoviesAndSeriesModel>();

            CreateMap<CreateMovieModel, MoviesAndSeries>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.Genres, a => a.Ignore())
                .ForMember(a => a.Actors, a => a.Ignore());

            CreateMap<MoviesAndSeries, ListMoviesWithDetailsModel>()
                .ForMember(a => a.Image, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.MovieOrSeriesImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<Watch, ListMoviesWithDetailsModel>()
                .ForMember(a => a.Id, a => a.MapFrom(src => src.MovieSeriesId))
                .ForMember(a => a.Title, a => a.MapFrom(src => src.MovieSeries.Title))
                .ForMember(a => a.Description, a => a.MapFrom(src => src.MovieSeries.Description))
                .ForMember(a => a.IsSeries, a => a.MapFrom(src => src.MovieSeries.IsSeries))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.MovieSeries.Genres))
                .ForMember(a => a.Image, a => a.MapFrom(src => src.MovieSeries.MovieOrSeriesImage.Image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.MovieSeries.MovieOrSeriesImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieSeries.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<LikeMovie, ListMoviesWithDetailsModel>()
                .ForMember(a => a.Id, a => a.MapFrom(src => src.MovieSeriesId))
                .ForMember(a => a.Title, a => a.MapFrom(src => src.MovieSeries.Title))
                .ForMember(a => a.Description, a => a.MapFrom(src => src.MovieSeries.Description))
                .ForMember(a => a.IsSeries, a => a.MapFrom(src => src.MovieSeries.IsSeries))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.MovieSeries.Genres))
                .ForMember(a => a.Image, a => a.MapFrom(src => src.MovieSeries.MovieOrSeriesImage.Image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.MovieSeries.MovieOrSeriesImage.Image)) : ""))
                .ForMember(a => a.HasAvailableImage, a => a.MapFrom(src => src.MovieSeries.MovieOrSeriesImage.Image != null ? true : false));

            CreateMap<LikeMovie, MovieWithGenresModel>()
                .ForMember(a => a.Id, a => a.MapFrom(src => src.MovieSeriesId))
                .ForMember(a => a.Title, a => a.MapFrom(src => src.MovieSeries.Title))
                .ForMember(a => a.Genres, a => a.MapFrom(src => src.MovieSeries.Genres.Select(m => m.Name).ToList()));

            CreateMap<Review, MovieWithRatingModel>()
                .ForMember(a => a.Id, a => a.MapFrom(src => src.MovieSeriesId))
                .ForMember(a => a.Title, a => a.MapFrom(src => src.MovieSeries.Title));
        }
    }
}
