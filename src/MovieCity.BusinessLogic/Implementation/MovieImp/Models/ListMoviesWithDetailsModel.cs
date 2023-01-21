using MovieCity.BusinessLogic.Implementation.GenreImp.Models;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class ListMoviesWithDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<ListGenresModel> Genres { get; set; }
        public string Image { get; set; }
        public bool IsSeries { get; set; }
        public bool HasAvailableImage { get; set; }
    }
}
