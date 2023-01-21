using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class MovieWithRatingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
    }
}
