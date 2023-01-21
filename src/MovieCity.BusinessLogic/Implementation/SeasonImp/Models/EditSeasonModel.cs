using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp.Models
{
    public class EditSeasonModel
    {
        public Guid Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public Guid MovieSeriesId { get; set; }
    }
}
