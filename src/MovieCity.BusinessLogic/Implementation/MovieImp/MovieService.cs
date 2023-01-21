using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.BusinessLogic.Implementation.MovieImp.Validations;
using MovieCity.BusinessLogic.Implementation.ReviewImp;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;
using MovieCity.Entities.Enums;
using X.PagedList;

namespace MovieCity.BusinessLogic.Implementation.MovieImp
{
    public class MovieService : BaseService
    {
        private readonly MovieValidator movieValidator;
        private readonly EditMovieValidator editMovieValidator;
        private readonly GenreService genreService;
        private readonly ReviewService reviewService;

        public MovieService(ServiceDependencies serviceDependencies, GenreService genreService, ReviewService reviewService) : base(serviceDependencies)
        {
            this.movieValidator = new MovieValidator();
            this.editMovieValidator = new EditMovieValidator();
            this.genreService = genreService;
            this.reviewService = reviewService;
        }

        public async Task<IPagedList<ListMoviesAndSeriesModel>> GetMovies(string searchString, int pageNumber, int pageSize)
        {
            var movies = Mapper.ProjectTo<ListMoviesAndSeriesModel>(UnitOfWork.MoviesAndSeries.Get());

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            return movies.OrderBy(m => m.Title).ToPagedList(pageNumber, pageSize);
        }

        public IQueryable<ListMoviesAndSeriesModel> GetMoviesQueriable()
        {
            return Mapper.ProjectTo<ListMoviesAndSeriesModel>(UnitOfWork.MoviesAndSeries.Get());
        }

        public async Task<IPagedList<ListMoviesWithDetailsModel>> GetMoviesWithDetails(string searchString, 
            ShowTypes? showType, 
            Guid? genreFilter, 
            int pageNumber, 
            int pageSize)
        {
            var movies = Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.MoviesAndSeries.Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres));

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            switch (showType)
            {
                case ShowTypes.Movies:
                    {
                        movies = movies.Where(m => !m.IsSeries);
                        break;
                    }
                case ShowTypes.Series:
                    {
                        movies = movies.Where(m => m.IsSeries);
                        break;
                    }
                default:
                    break;
            }

            if(genreFilter != null && genreFilter != Guid.Empty)
            {
                movies = movies.Where(m => m.Genres.Any(g => g.Id == genreFilter));
            }

            return await movies.OrderBy(m => m.Title).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<ListMoviesWithDetailsModel>> GetWatchedMovies(bool watched, string searchString, int pageNumber, int pageSize)
        {
            var movies = Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.Watch.Get()
                .Include(m => m.MovieSeries)
                    .ThenInclude(m => m.MovieOrSeriesImage)
                .Include(m => m.MovieSeries)
                    .ThenInclude(m => m.Genres)
                .Where(m => m.UserId == CurrentUser.Id && m.IsAlreadyWatched == watched));

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            return await movies.OrderBy(m => m.Title).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<ListMoviesWithDetailsModel>> GetLikedMovies(string searchString, int pageNumber, int pageSize)
        {
            var movies = Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.LikeMovie.Get()
                .Include(m => m.MovieSeries)
                    .ThenInclude(m => m.MovieOrSeriesImage)
                .Include(m => m.MovieSeries)
                    .ThenInclude(m => m.Genres)
                .Where(m => m.UserId == CurrentUser.Id && m.IsLiked == true));

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            return await movies.OrderBy(m => m.Title).ToPagedListAsync(pageNumber, pageSize);
        }

        private async Task<List<ListMoviesWithDetailsModel>> GetMostLikedMovies()
        {
            var moviesIds = await UnitOfWork.LikeMovie.Get()
                .Where(m => m.IsLiked == true)
                .GroupBy(m => m.MovieSeriesId)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Select(g => g.Key)
                .Take(3).ToListAsync();

            return await Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.MoviesAndSeries.Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres))
                .Where(m => moviesIds.Contains(m.Id))
                .OrderBy(m => m.Title).ToListAsync();
        }

        private async Task<List<ListMoviesWithDetailsModel>> GetMostPopularMovies()
        {
            var moviesIds = await UnitOfWork.Watch.Get()
                .Where(m => m.IsAlreadyWatched == true)
                .GroupBy(m => m.MovieSeriesId)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Select(g => g.Key)
                .Take(3).ToListAsync();

            return await Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.MoviesAndSeries.Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres))
                .Where(m => moviesIds.Contains(m.Id))
                .OrderBy(m => m.Title).ToListAsync();
        }

        public async Task<HomeMoviesModel> GetPopularAndLikedMovies()
        {
            var model = new HomeMoviesModel();

            model.MostPopularMovies = await GetMostPopularMovies();
            model.MostLikedMovies = await GetMostLikedMovies();

            return model;
        }

        public async Task<List<ListMoviesWithDetailsModel>> GetRecommendedMovies()
        {
            var favoriteGenresIds = await genreService.GetFavoriteGenresIds(CurrentUser.Id);
            var watchedMoviesIds = await UnitOfWork.Watch.Get()
                .Where(w => w.UserId == CurrentUser.Id && w.IsAlreadyWatched == true)
                .Select(w => w.MovieSeriesId)
                .ToListAsync();

            var moviesIds = await UnitOfWork.Watch.Get()
                .Include(w => w.MovieSeries)
                    .ThenInclude(w => w.Genres)
                .Where(w => w.MovieSeries.Genres.Any(g => favoriteGenresIds.Contains(g.Id)) && !watchedMoviesIds.Contains(w.MovieSeriesId))
                .GroupBy(m => m.MovieSeriesId)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Select(g => g.Key)
                .Take(4).ToListAsync();

            return await Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.MoviesAndSeries.Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres))
                .Where(m => moviesIds.Contains(m.Id))
                .OrderBy(m => m.Title).ToListAsync();
        }

        public async Task CreateNewMovie(CreateMovieModel model)
        {
            model.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            model.AllGenres = await genreService.GetGenres();
            movieValidator.Validate(model).ThenThrow(model);

            model.Genres = model.Genres ?? new List<Guid>();
            var genres = await UnitOfWork.Genres.Get().Where(g => model.Genres.Contains(g.Id)).ToListAsync();

            model.Actors = model.Actors ?? new List<Guid>();
            var actors = await UnitOfWork.Actors.Get().Where(a => model.Actors.Contains(a.Id)).ToListAsync();

            var newMovie = Mapper.Map<MoviesAndSeries>(model);

            newMovie.IsSeries = false;
            newMovie.Actors = actors;
            newMovie.Genres = genres;
            newMovie.Movie = new Movie
            {
                Duration = model.Duration,
                ReleaseDate = model.ReleaseDate
            };

            if (model.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newMovie.MovieOrSeriesImage = new MovieOrSeriesImage
                    {
                        Image = fileBytes
                    };
                }
            }

            UnitOfWork.MoviesAndSeries.Insert(newMovie);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<EditMovieModel> GetEditMovieModelById(Guid id)
        {
            var movieOrSeries = await UnitOfWork.MoviesAndSeries
                 .Get()
                 .Include(m => m.Movie)
                 .Include(m => m.Actors)
                 .Include(m => m.Genres)
                 .Include(m => m.MovieOrSeriesImage)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (movieOrSeries == null)
            {
                throw new NotFoundErrorException();
            }

            var movieModel = Mapper.Map<EditMovieModel>(movieOrSeries);

            if (movieOrSeries.MovieOrSeriesImage != null)
            {
                movieModel.ActualImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(movieOrSeries.MovieOrSeriesImage.Image));
            }

            movieModel.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            movieModel.AllGenres = await genreService.GetGenres();

            return movieModel;
        }

        public async Task<ViewMovieDetailsModel> GetMovieDetailsModelById(Guid id)
        {
            var movieOrSeries = await UnitOfWork.MoviesAndSeries
                .Get()
                .Include(m => m.Movie)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .Include(m => m.MovieOrSeriesImage)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (movieOrSeries == null)
            {
                throw new NotFoundErrorException();
            }

            var movieModel = Mapper.Map<ViewMovieDetailsModel>(movieOrSeries);

            if(movieOrSeries?.MovieOrSeriesImage != null)
            {
                movieModel.Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(movieOrSeries.MovieOrSeriesImage.Image));
            }

            return movieModel;
        }

        public async Task<MovieDetailsModel> GetMovieModelById(Guid id)
        {
            var movieOrSeries = await UnitOfWork.MoviesAndSeries
                .Get()
                .Where(m => m.Id == id)
                .Include(m => m.Movie)
                .Include(m => m.Actors)
                    .ThenInclude(m => m.ActorImage)
                .Include(m => m.Genres)
                .Include(m => m.MovieOrSeriesImage)
                .FirstOrDefaultAsync();

            if (movieOrSeries == null)
            {
                throw new NotFoundErrorException();
            }

            var movieModel = Mapper.Map<MovieDetailsModel>(movieOrSeries);

            if (movieOrSeries?.MovieOrSeriesImage != null)
            {
                movieModel.Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(movieOrSeries.MovieOrSeriesImage.Image));
                movieModel.HasAvailableImage = true;
            }
            else
            {
                movieModel.HasAvailableImage = false;
            }

            var likes = UnitOfWork.LikeMovie.Get().Where(m => m.MovieSeriesId == id && !m.User.IsDeleted);
            movieModel.LikesNo = likes.Where(m => m.IsLiked).Count();
            movieModel.DislikesNo = likes.Where(m => !m.IsLiked).Count();

            movieModel.IsLiked = likes.Where(w => w.UserId == CurrentUser.Id && w.IsLiked).FirstOrDefault() != null;
            movieModel.IsDisliked = likes.Where(w => w.UserId == CurrentUser.Id && !w.IsLiked).FirstOrDefault() != null;

            var watches = UnitOfWork.Watch.Get().Where(m => m.MovieSeriesId == id && m.UserId == CurrentUser.Id);
            movieModel.IsInWatched = watches.Where(w => w.IsAlreadyWatched).FirstOrDefault() != null;
            movieModel.IsInWatchlist = watches.Where(w => !w.IsAlreadyWatched).FirstOrDefault() != null;

            movieModel.Reviews = await reviewService.GetReviewsForMovie(id);

            var image = await UnitOfWork.UserImages.Get().FirstOrDefaultAsync(u => u.UserId == CurrentUser.Id);
            if(image != null)
            {
                movieModel.CurrentUserImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image.Image));
            } 

            return movieModel;
        }

        public async Task<List<MovieWithGenresModel>> GetRecentLikedMovies(Guid id)
        {
            return await Mapper.ProjectTo<MovieWithGenresModel>(UnitOfWork.LikeMovie.Get()
                .Include(m => m.MovieSeries)
                    .ThenInclude(m => m.Genres)
                .Where(m => m.UserId == id)
                .OrderByDescending(m => m.Date)
                .Take(3))
                .ToListAsync();
        }

        public async Task<bool> UpdateMovie(EditMovieModel model)
        {
            model.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            model.AllGenres = await genreService.GetGenres();
            editMovieValidator.Validate(model).ThenThrow(model);

            var movieToUpdate = await UnitOfWork.MoviesAndSeries
                .Get()
                .Include(m => m.Movie)
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            model.Genres = model.Genres ?? new List<Guid>();
            var genres = await UnitOfWork.Genres.Get().Where(g => model.Genres.Contains(g.Id)).ToListAsync();

            model.Actors = model.Actors ?? new List<Guid>();
            var actors = await UnitOfWork.Actors.Get().Where(a => model.Actors.Contains(a.Id)).ToListAsync();

            if (movieToUpdate == null) return false;

            movieToUpdate.Title = model.Title;
            movieToUpdate.Description = model.Description;
            movieToUpdate.Movie.Duration = model.Duration;
            movieToUpdate.Movie.ReleaseDate = model.ReleaseDate;
            movieToUpdate.Genres = genres;
            movieToUpdate.Actors = actors;

            if (model.NewImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.NewImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    if (movieToUpdate.MovieOrSeriesImage != null)
                    {
                        movieToUpdate.MovieOrSeriesImage.Image = fileBytes;
                    }
                    else
                    {
                        movieToUpdate.MovieOrSeriesImage = new MovieOrSeriesImage
                        {
                            Image = fileBytes
                        };
                    }
                }
            }
            else
            {
                if (model.DeleteActualImage)
                {
                    movieToUpdate.MovieOrSeriesImage = null;
                }
            }

            UnitOfWork.MoviesAndSeries.Update(movieToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMovie(string id)
        {
            var GuidId = Guid.Parse(id);
            var movieToDelete = await UnitOfWork.MoviesAndSeries.Get()
                .Include(e => e.Movie)
                .Include(e => e.Genres)
                .Include(e => e.Actors)
                .Include(e => e.Reviews)
                    .ThenInclude(e => e.Posts)
                        .ThenInclude(e => e.LikePosts)
                .Include(e => e.Reviews)
                    .ThenInclude(e => e.Posts)
                        .ThenInclude(e => e.Comments)
                .Include(e => e.Watches)
                .Include(e => e.LikeMovies)
                .Include(e => e.MovieOrSeriesImage)
                .FirstOrDefaultAsync(g => g.Id == GuidId);

            if (movieToDelete == null) return false;

            movieToDelete.Genres.Clear();
            movieToDelete.Actors.Clear();
            movieToDelete.Movie = null;
            movieToDelete.MovieOrSeriesImage = null;

            var reviewsToDelete = movieToDelete.Reviews.ToList();

            var postsToDelete = new List<Post>();
            reviewsToDelete.ForEach(r => postsToDelete.AddRange(r.Posts));

            var commentsToDelete = new List<Comment>();
            postsToDelete.ForEach(r => commentsToDelete.AddRange(r.Comments));
            UnitOfWork.Comments.DeleteRange(commentsToDelete);

            var likesPostToDelete = new List<LikePost>();
            postsToDelete.ForEach(r => likesPostToDelete.AddRange(r.LikePosts));
            UnitOfWork.LikePost.DeleteRange(likesPostToDelete);

            UnitOfWork.Posts.DeleteRange(postsToDelete);
            UnitOfWork.Reviews.DeleteRange(reviewsToDelete);

            var likesToDelete = movieToDelete.LikeMovies.ToList();
            UnitOfWork.LikeMovie.DeleteRange(likesToDelete);

            var watchesToDelete = movieToDelete.Watches.ToList();
            UnitOfWork.Watch.DeleteRange(watchesToDelete);

            UnitOfWork.MoviesAndSeries.Delete(movieToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<ListMoviesWithDetailsModel>> GetMoviesForActor(Guid actorId)
        {
            return await Mapper.ProjectTo<ListMoviesWithDetailsModel>(UnitOfWork.MoviesAndSeries.Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres)
                .Include(m=> m.Actors)
                .Where(m => m.Actors.Select(a => a.Id).Contains(actorId)))
                .OrderBy(m => m.Title).Take(3).ToListAsync();
        }
    }
}
