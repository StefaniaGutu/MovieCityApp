using MovieCity.BusinessLogic.Implementation.EpisodeImp.Models;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp.Models
{
    public class ListSeasonModel
    {
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }

        public virtual List<ListEpisodeModel> Episodes { get; set; }
    }
}
