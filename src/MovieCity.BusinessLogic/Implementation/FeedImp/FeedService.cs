using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.Account;
using MovieCity.BusinessLogic.Implementation.FeedImp.Models;
using MovieCity.BusinessLogic.Implementation.FriendImp;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.FeedImp
{
    public class FeedService : BaseService
    {
        private readonly FriendService friendService;
        private readonly UserAccountService userService;
        public FeedService(ServiceDependencies serviceDependencies, FriendService friendService, UserAccountService userService) : base(serviceDependencies)
        {
            this.friendService = friendService;
            this.userService = userService;
        }

        public async Task<FeedModel> GetPaginatedPosts(int page, int takePostNo)
        {
            var skipPostNo = page * takePostNo;

            var model = new FeedModel();

            model.ListPostsModels = await Mapper.ProjectTo<ListPostsModel>(UnitOfWork.Posts.Get()
                .Include(p => p.Comments)
                .Include(p => p.Review)
                    .ThenInclude(p => p.MovieSeries)
                     .ThenInclude(m => m.MovieOrSeriesImage)
                .Include(p => p.Review)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.UserImage)
                .Where(p => (friendService.GetFriendsIds().Contains(p.UserId) && !p.User.IsDeleted) || p.UserId == CurrentUser.Id))
                .OrderByDescending(p => p.Date)
                .Skip(skipPostNo)
                .Take(takePostNo)
                .ToListAsync();
            
            foreach(var post in model.ListPostsModels)
            {
                post.CommentsLeft = UnitOfWork.Comments.Get()
                    .Include(c => c.User)
                    .Where(c => c.PostId == post.Id && !c.User.IsDeleted).Count() > 3;

                if (post.CommentsLeft)
                {
                    post.Comments = post.Comments.Where(c => !c.User.IsDeleted).Take(3).ToList();
                }

                if (post.Review.UserImage == null)
                {
                    post.Review.UserImage = userService.GetDefaultUserImage();
                }

                foreach(var comm in post.Comments)
                {
                    if (comm.User.Image == null)
                    {
                        comm.User.Image = userService.GetDefaultUserImage();
                    }
                }

                post.LikesNo = UnitOfWork.LikePost.Get().Where(m => m.PostId == post.Id && !m.User.IsDeleted && m.IsLiked).Count();
                post.DislikesNo = UnitOfWork.LikePost.Get().Where(m => m.PostId == post.Id && !m.User.IsDeleted && !m.IsLiked).Count();
                post.IsLikedByCurrentUser = await GetLike(post.Id, true);
                post.IsDislikedByCurrentUser = await GetLike(post.Id, false);
            }

            model.PostsLeft = UnitOfWork.Posts.Get()
                .Where(p => (friendService.GetFriendsIds().Contains(p.UserId) && !p.User.IsDeleted) || p.UserId == CurrentUser.Id)
                .Count() - (skipPostNo + takePostNo) > 0 ? true : false;

            model.FirstTimeOnPage = page == 0;
            model.CurrentUserImage = userService.GetCurrentUserImage();
            model.RecentReceivedRequests = await friendService.GetRecentReceivedRequests();

            return model;
        }

        public async Task<CommentFeedModel> GetPaginatedComments(int page, int takeCommentNo, Guid postId)
        {
            var skipCommentNo = page * takeCommentNo;

            var model = new CommentFeedModel();

            model.ListCommentsModels = await Mapper.ProjectTo<CommentModel>(UnitOfWork.Comments.Get()
                .Include(c => c.User)
                .Where(c => c.PostId == postId && !c.User.IsDeleted))
                .Skip(skipCommentNo).Take(takeCommentNo)
                .ToListAsync();


            model.CommentsLeft = UnitOfWork.Comments.Get().Where(c => c.PostId == postId && !c.User.IsDeleted).Count() - (skipCommentNo + takeCommentNo) > 0 ? true : false;

            foreach(var comm in model.ListCommentsModels)
            {
                if(comm.User.Image == null)
                    comm.User.Image = userService.GetDefaultUserImage();
            }

            return model;
        }

        public async Task<bool> GetLike(Guid postId, bool isLiked)
        {
            return await UnitOfWork.LikePost.Get().FirstOrDefaultAsync(w => w.PostId == postId && w.IsLiked == isLiked && w.UserId == CurrentUser.Id) != null;
        }

        public async Task AddLike(Guid postId, bool isLiked)
        {
            var oldLike = await UnitOfWork.LikePost.Get().FirstOrDefaultAsync(w => w.PostId == postId && w.UserId == CurrentUser.Id);

            if (oldLike == null)
            {
                var newLike = new LikePost
                {
                    UserId = CurrentUser.Id,
                    PostId = postId,
                    IsLiked = isLiked
                };

                UnitOfWork.LikePost.Insert(newLike);
            }
            else
            {
                if (oldLike.IsLiked != isLiked)
                {
                    oldLike.IsLiked = isLiked;
                    UnitOfWork.LikePost.Update(oldLike);
                }
                else
                {
                    UnitOfWork.LikePost.Delete(oldLike);
                }
            }
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Guid postId, string postText)
        {
            var postToUpdate = await UnitOfWork.Posts.Get().FirstOrDefaultAsync(g => g.Id == postId);

            if (postToUpdate == null) return false;

            postToUpdate.PostText = postText;

            UnitOfWork.Posts.Update(postToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public Guid AddComment(string commentText, Guid postId)
        {
            var newComment = new Comment {
                Id = Guid.NewGuid(),
                CommentText = commentText,
                PostId = postId,
                UserId = CurrentUser.Id,
                Date = DateTime.Now
            };

            UnitOfWork.Comments.Insert(newComment);
            UnitOfWork.SaveChanges();

            return newComment.Id;
        }

        public async Task<bool> DeleteComment(string id)
        {
            var GuidId = Guid.Parse(id);
            var commentToDelete = await UnitOfWork.Comments.Get()
                .FirstOrDefaultAsync(g => g.Id == GuidId);

            if (commentToDelete == null) return false;

            UnitOfWork.Comments.Delete(commentToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(string id)
        {
            var GuidId = Guid.Parse(id);
            var postToDelete = await UnitOfWork.Posts.Get()
                .Include(p => p.Comments)
                .Include(p => p.LikePosts)
                .FirstOrDefaultAsync(g => g.Id == GuidId);

            if (postToDelete == null) return false;

            var likesToDelete = postToDelete.LikePosts.ToList();
            UnitOfWork.LikePost.DeleteRange(likesToDelete);

            var commentsToDelete = postToDelete.Comments.ToList();
            UnitOfWork.Comments.DeleteRange(commentsToDelete);

            UnitOfWork.Posts.Delete(postToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
