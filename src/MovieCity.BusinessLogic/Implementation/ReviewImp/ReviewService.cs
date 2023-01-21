using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.BusinessLogic.Implementation.ReviewImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.ReviewImp
{
    public class ReviewService : BaseService
    {
        public ReviewService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public async Task<List<ReviewListModel>> GetAllReviews()
        {
            return await Mapper.ProjectTo<ReviewListModel>(UnitOfWork.Reviews.Get()
                .Include(r => r.User)
                .Include(r => r.MovieSeries)
                .Where(m => m.User.Id == CurrentUser.Id && !m.User.IsDeleted))
                .OrderByDescending(m => m.Date).ToListAsync();
        }

        public Guid CreateReview(Guid movieId, int rating, string reviewText, bool showInFeed, string postText)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Rating = rating,
                ReviewText = reviewText,
                UserId = CurrentUser.Id,
                MovieSeriesId = movieId,
                Date = DateTime.Now,
                ShowInFeed = showInFeed
            };

            if(showInFeed == true)
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    UserId = CurrentUser.Id,
                    ReviewId = review.Id,
                    PostText = postText,
                    Date = review.Date
                };

                UnitOfWork.Posts.Insert(post);
            }

            UnitOfWork.Reviews.Insert(review);
            UnitOfWork.SaveChanges();

            return review.Id;
        }

        public async Task<List<ReviewModel>> GetReviewsForMovie(Guid movieId)
        {
            var reviews = await Mapper.ProjectTo<ReviewModel>(UnitOfWork.Reviews.Get()
                .Include(r => r.User)
                    .ThenInclude(u => u.UserImage)
                .Where(m => m.MovieSeriesId == movieId && !m.User.IsDeleted))
                .OrderBy(m => m.Date).ToListAsync();

            foreach (var review in reviews)
            {
                if (review.UserImage == null)
                {
                    using (var fs = new FileStream("C:\\Users\\stefania.gutu\\Desktop\\Academie\\Proiect - MovieCity\\StefaniaGutu\\src\\MovieCity.WebApp\\wwwroot\\no-profile-picture.png", FileMode.Open, FileAccess.Read))
                    {
                        byte[] userImage = new byte[fs.Length];
                        fs.Read(userImage, 0, Convert.ToInt32(fs.Length));
                        review.UserImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(userImage));
                    }
                }
            }

            return reviews;
        }

        public async Task<List<MovieWithRatingModel>> GetRecentReviews(Guid id)
        {
            return await Mapper.ProjectTo<MovieWithRatingModel>(UnitOfWork.Reviews.Get()
                .Include(m => m.MovieSeries)
                .Where(m => m.UserId == id)
                .OrderByDescending(m => m.Date)
                .Take(3))
                .ToListAsync();
        }

        public async Task<bool> DeleteReview(string id)
        {
            var guidId = Guid.Parse(id);
            var reviewToDelete = await UnitOfWork.Reviews.Get()
                .Include(a => a.Posts)
                .ThenInclude(a => a.Comments)
                .Include(a => a.Posts)
                .ThenInclude(a => a.LikePosts)
                .FirstOrDefaultAsync(g => g.Id == guidId);

            if (reviewToDelete == null) return false;

            if (CurrentUser.Roles.Any(r => r == "Admin") || CurrentUser.Id == reviewToDelete.UserId)
            {
                var postsToDelete = reviewToDelete.Posts.ToList();

                var comments = new List<Comment>();
                var postLikes = new List<LikePost>();
                foreach (var post in postsToDelete)
                {
                    comments.AddRange(post.Comments);
                    postLikes.AddRange(post.LikePosts);
                }

                UnitOfWork.Comments.DeleteRange(comments);
                UnitOfWork.LikePost.DeleteRange(postLikes);
                UnitOfWork.Posts.DeleteRange(postsToDelete);

                UnitOfWork.Reviews.Delete(reviewToDelete);
                await UnitOfWork.SaveChangesAsync();

                return true;
            }
                
            return false;
        }
    }
}
