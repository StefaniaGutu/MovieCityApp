using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.WatchAndLikeImp
{
    public class WatchAndLikeService : BaseService
    {
        public WatchAndLikeService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public async Task<bool> AddWatch(Guid movieId, bool isAlreadyWatched)
        {
            var oldWatch = await UnitOfWork.Watch.Get()
                .Include(w => w.MovieSeries)
                .FirstOrDefaultAsync(w => w.MovieSeriesId == movieId && w.IsAlreadyWatched == isAlreadyWatched && w.UserId == CurrentUser.Id );
            if (oldWatch == null)
            {
                var newWatch = new Watch
                {
                    UserId = CurrentUser.Id,
                    MovieSeriesId = movieId,
                    IsAlreadyWatched = isAlreadyWatched,
                    Date = DateTime.Now
                };

                UnitOfWork.Watch.Insert(newWatch);
            }
            else
            {
                UnitOfWork.Watch.Delete(oldWatch);
            }
            await UnitOfWork.SaveChangesAsync();

            return UnitOfWork.MoviesAndSeries.Get().FirstOrDefault(m => m.Id == movieId).IsSeries;
        }

        public async Task<bool> AddLike(Guid movieId, bool isLiked)
        {
            var oldLike = await UnitOfWork.LikeMovie.Get().FirstOrDefaultAsync(w => w.MovieSeriesId == movieId && w.UserId == CurrentUser.Id);
           
            if (oldLike == null)
            {
                var newLike = new LikeMovie
                {
                    UserId = CurrentUser.Id,
                    MovieSeriesId = movieId,
                    IsLiked = isLiked,
                    Date = DateTime.Now
                };

                UnitOfWork.LikeMovie.Insert(newLike);
            }
            else
            {
                if(oldLike.IsLiked != isLiked)
                {
                    oldLike.IsLiked = isLiked;
                    UnitOfWork.LikeMovie.Update(oldLike);
                }
                else
                {
                    UnitOfWork.LikeMovie.Delete(oldLike);
                }
            }
            await UnitOfWork.SaveChangesAsync();
            return UnitOfWork.MoviesAndSeries.Get().FirstOrDefault(m => m.Id == movieId).IsSeries;
        }
    }
}
