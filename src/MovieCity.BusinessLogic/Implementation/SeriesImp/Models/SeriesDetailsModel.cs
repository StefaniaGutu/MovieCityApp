using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.ReviewImp.Models;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Models;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Models
{
    public class SeriesDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public virtual List<ListSeasonModel> Seasons { get; set; }
        public List<String> Genres { get; set; }
        public List<ListActorWithImageModel> Actors { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
        public int LikesNo { get; set; }
        public int DislikesNo { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public bool IsInWatchlist { get; set; }
        public bool IsInWatched { get; set; }
        public List<ReviewModel> Reviews { get; set; }
        public string? CurrentUserImage { get; set; }
    }
}
