using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Models
{
    public class EditSeriesModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? Genres { get; set; }
        public List<Guid>? Actors { get; set; }
        public IFormFile? NewImage { get; set; }
        public string ActualImage { get; set; }
        public bool HasAvailableImage { get; set; }
        public List<ListGenresModel> AllGenres { get; set; }
        public List<ListActorsModel> AllActors { get; set; }
    }
}
