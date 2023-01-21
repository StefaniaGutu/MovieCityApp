namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class ViewMovieDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<String> Genres { get; set; }
        public List<String> Actors { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
    }
}
