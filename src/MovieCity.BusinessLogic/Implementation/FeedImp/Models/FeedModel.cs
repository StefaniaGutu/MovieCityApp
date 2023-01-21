using MovieCity.BusinessLogic.Implementation.FriendImp.Models;

namespace MovieCity.BusinessLogic.Implementation.FeedImp.Models
{
    public class FeedModel
    {
        public bool FirstTimeOnPage { get; set; }
        public bool PostsLeft { get; set; }
        public List<ListPostsModel> ListPostsModels { get; set; }
        public string? CurrentUserImage { get; set; }
        public List<ListFriendsModel> RecentReceivedRequests { get; set; }
    }
}
