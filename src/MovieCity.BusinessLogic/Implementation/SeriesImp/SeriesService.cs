using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp;
using MovieCity.BusinessLogic.Implementation.ReviewImp;
using MovieCity.BusinessLogic.Implementation.SeasonImp;
using MovieCity.BusinessLogic.Implementation.SeriesImp.Models;
using MovieCity.BusinessLogic.Implementation.SeriesImp.Validations;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp
{
    public class SeriesService : BaseService
    {
        private readonly SeriesValidator seriesValidator;
        private readonly EditSeriesValidator editSeriesValidator;
        private readonly ReviewService reviewService;
        private readonly SeasonService seasonService;
        private readonly GenreService genreService;

        public SeriesService(ServiceDependencies serviceDependencies, ReviewService reviewService, SeasonService seasonService, GenreService genreService) 
            : base(serviceDependencies)
        {
            seriesValidator = new SeriesValidator();
            editSeriesValidator = new EditSeriesValidator();
            this.reviewService = reviewService;
            this.seasonService = seasonService;
            this.genreService = genreService;
        }

        public async Task<ViewSeriesDetailsModel> GetSeriesDetailsModelById(Guid id)
        {
            var series = await UnitOfWork.MoviesAndSeries
                .Get()
                .Include(s => s.Genres)
                .Include(s => s.Actors)
                .Include(s => s.MovieOrSeriesImage)
                .Include(s => s.Seasons)
                    .ThenInclude(s => s.Episodes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (series == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<ViewSeriesDetailsModel>(series);

            if (series.MovieOrSeriesImage != null)
            {
                model.Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(series.MovieOrSeriesImage.Image));
            }

            return model;
        }

        public async Task<SeriesDetailsModel> GetSeriesDetailsById(Guid id)
        {
            var series = await UnitOfWork.MoviesAndSeries
                .Get()
                .Where(s => s.Id == id)
                .Include(s => s.Genres)
                .Include(s => s.Actors)
                    .ThenInclude(a => a.ActorImage)
                .Include(s => s.MovieOrSeriesImage)
                .FirstOrDefaultAsync();

            if (series == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<SeriesDetailsModel>(series);

            if (series.MovieOrSeriesImage != null)
            {
                model.Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(series.MovieOrSeriesImage.Image));
            }

            var likes = UnitOfWork.LikeMovie.Get().Where(m => m.MovieSeriesId == id && !m.User.IsDeleted);
            model.LikesNo =  likes.Where(m => m.IsLiked).Count();
            model.DislikesNo = likes.Where(m => !m.IsLiked).Count();

            model.IsLiked = likes.Where(w => w.UserId == CurrentUser.Id && w.IsLiked).FirstOrDefault() != null;
            model.IsDisliked = likes.Where(w => w.UserId == CurrentUser.Id && !w.IsLiked).FirstOrDefault() != null;

            var watches = UnitOfWork.Watch.Get().Where(m => m.MovieSeriesId == id && m.UserId == CurrentUser.Id);
            model.IsInWatched = watches.Where(w => w.IsAlreadyWatched).FirstOrDefault() != null;
            model.IsInWatchlist = watches.Where(w => !w.IsAlreadyWatched).FirstOrDefault() != null;

            model.Reviews = await reviewService.GetReviewsForMovie(id);
            model.Seasons = await seasonService.GetSeasonsForSeries(id);

            var image = await UnitOfWork.UserImages.Get().FirstOrDefaultAsync(u => u.UserId == CurrentUser.Id);
            if (image != null)
            {
                model.CurrentUserImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image.Image));
            }

            return model;
        }

        public async Task<EditSeriesModel> GetEditSeriesModelById(Guid id)
        {
            var series = await UnitOfWork.MoviesAndSeries
                .Get()
                .Include(s => s.Genres)
                .Include(s => s.Actors)
                .Include(s => s.MovieOrSeriesImage)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (series == null)
            {
                throw new NotFoundErrorException();
            } 

            var model = Mapper.Map<EditSeriesModel>(series);

            if (series.MovieOrSeriesImage != null)
            {
                model.ActualImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(series.MovieOrSeriesImage.Image));
            }

            model.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            model.AllGenres = await genreService.GetGenres();

            return model;
        }

        public async Task CreateNewSeries(CreateSeriesModel model)
        {
            model.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            model.AllGenres = await genreService.GetGenres();
            seriesValidator.Validate(model).ThenThrow(model);

            model.Genres = model.Genres ?? new List<Guid>();
            var genres = await UnitOfWork.Genres.Get().Where(g => model.Genres.Contains(g.Id)).ToListAsync();

            model.Actors = model.Actors ?? new List<Guid>();
            var actors = await UnitOfWork.Actors.Get().Where(a => model.Actors.Contains(a.Id)).ToListAsync();

            var newSeries = Mapper.Map<MoviesAndSeries>(model);

            newSeries.IsSeries = true;
            newSeries.Genres = genres;
            newSeries.Actors = actors;

            if (model.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newSeries.MovieOrSeriesImage = new MovieOrSeriesImage
                    {
                        Image = fileBytes
                    };
                }
            }

            UnitOfWork.MoviesAndSeries.Insert(newSeries);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateSeries(EditSeriesModel model)
        {
            model.AllActors = await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
            model.AllGenres = await genreService.GetGenres();
            editSeriesValidator.Validate(model).ThenThrow(model);

            var seriesToUpdate = await UnitOfWork.MoviesAndSeries
                .Get()
                .Include(m => m.MovieOrSeriesImage)
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (seriesToUpdate == null) return false;

            model.Genres = model.Genres ?? new List<Guid>();
            var genres = await UnitOfWork.Genres.Get().Where(g => model.Genres.Contains(g.Id)).ToListAsync();

            model.Actors = model.Actors ?? new List<Guid>();
            var actors = await UnitOfWork.Actors.Get().Where(a => model.Actors.Contains(a.Id)).ToListAsync();

            seriesToUpdate.Title = model.Title;
            seriesToUpdate.Description = model.Description;
            seriesToUpdate.Genres = genres;
            seriesToUpdate.Actors = actors;

            if (model.NewImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.NewImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    if (seriesToUpdate.MovieOrSeriesImage != null)
                    {
                        seriesToUpdate.MovieOrSeriesImage.Image = fileBytes;
                    }   
                    else
                    {
                        seriesToUpdate.MovieOrSeriesImage = new MovieOrSeriesImage
                        {
                            Image = fileBytes
                        };
                    }
                }
            }

            UnitOfWork.MoviesAndSeries.Update(seriesToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSeries(string id)
        {
            var GuidId = Guid.Parse(id);
            var seasonsToDelete = await UnitOfWork.Seasons.Get()
                .Where(g => g.MovieSeriesId == GuidId)
                .ToListAsync();

            foreach (var season in seasonsToDelete)
            {
                var episodesToDelete = await UnitOfWork.Episodes.Get()
                    .Where(g => g.SeasonId == season.Id).ToListAsync();
                UnitOfWork.Episodes.DeleteRange(episodesToDelete);
            }

            UnitOfWork.Seasons.DeleteRange(seasonsToDelete);

            var serieToDelete = await UnitOfWork.MoviesAndSeries.Get()
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
                .Include(e => e.Seasons)
                    .ThenInclude(s=> s.Episodes)
                .FirstOrDefaultAsync(g => g.Id == GuidId);

            if (serieToDelete == null) return false;

            serieToDelete.Genres.Clear();
            serieToDelete.Actors.Clear();
            serieToDelete.Seasons.Clear();
            serieToDelete.MovieOrSeriesImage = null;

            var reviewsToDelete = serieToDelete.Reviews.ToList();

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

            var likesToDelete = serieToDelete.LikeMovies.ToList();
            UnitOfWork.LikeMovie.DeleteRange(likesToDelete);

            var watchesToDelete = serieToDelete.Watches.ToList();
            UnitOfWork.Watch.DeleteRange(watchesToDelete);

            UnitOfWork.MoviesAndSeries.Delete(serieToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
