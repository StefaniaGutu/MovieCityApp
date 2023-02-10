using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.ReviewImp.Models;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class MovieDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<String> Genres { get; set; }
        public List<ListActorWithImageModel> Actors { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public int LikesNo { get; set; }
        public int DislikesNo { get; set; }
        public bool IsInWatchlist { get; set; }
        public bool IsInWatched { get; set; }
        public List<ReviewModel> Reviews {get; set; }
        //public string? CurrentUserImage { get; set; }
    }
}
