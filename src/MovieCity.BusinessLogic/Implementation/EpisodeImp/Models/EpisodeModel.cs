namespace MovieCity.BusinessLogic.Implementation.EpisodeImp.Models
{
    public class EpisodeModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Duration { get; set; }
        public int EpisodeNo { get; set; }
        public Guid SeasonId { get; set; }
        public Guid SeriesId { get; set; }
    }
}
