using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class ListMoviesAndSeriesModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsSeries { get; set; }
    }
}
