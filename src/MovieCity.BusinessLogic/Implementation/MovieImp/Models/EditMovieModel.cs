using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class EditMovieModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Guid>? Genres { get; set; }
        public List<Guid>? Actors { get; set; }
        public IFormFile? NewImage { get; set; }
        public string ActualImage { get; set; }
        public bool HasAvailableImage { get; set; }
        public bool DeleteActualImage { get; set; }
        public List<ListGenresModel> AllGenres { get; set; }
        public List<ListActorsModel> AllActors { get; set; }
    }
}
