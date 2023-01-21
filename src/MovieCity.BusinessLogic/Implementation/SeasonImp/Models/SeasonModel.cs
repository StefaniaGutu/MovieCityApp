using MovieCity.BusinessLogic.Implementation.EpisodeImp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp.Models
{
    public class SeasonModel
    {
        public DateTime ReleaseDate { get; set; }
        public string? Name { get; set; }
        public string SeriesTitle { get; set; }
        public Guid SeriesId { get; set; }
        public string SeriesName { get; set; }
    }
}
