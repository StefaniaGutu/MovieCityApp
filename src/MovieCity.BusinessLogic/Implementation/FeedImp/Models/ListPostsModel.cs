using MovieCity.BusinessLogic.Implementation.ReviewImp.Models;

namespace MovieCity.BusinessLogic.Implementation.FeedImp.Models
{
    public class ListPostsModel
    {
        public Guid Id { get; set; }
        public bool IsReview { get; set; }
        public ReviewModel Review { get; set; }
        public string PostText { get; set; } = null!;
        public DateTime Date { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        public bool HasAvailableImage { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public bool CommentsLeft { get; set; }
        public int LikesNo { get; set; }
        public int DislikesNo { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsDislikedByCurrentUser { get; set; }
    }
}
