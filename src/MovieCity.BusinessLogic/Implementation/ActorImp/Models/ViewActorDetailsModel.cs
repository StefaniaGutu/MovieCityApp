using MovieCity.BusinessLogic.Implementation.MovieImp.Models;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Models
{
    public class ViewActorDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
        public List<ListMoviesWithDetailsModel> MoviesForActor { get; set; }
    }
}
