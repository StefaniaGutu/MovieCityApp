using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Models
{
    public class ViewSeriesDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
        public List<String> Genres { get; set; }
        public List<String> Actors { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
    }
}
