using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Models
{
    public class CreateSeriesModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Guid>? Genres { get; set; }
        public List<Guid>? Actors { get; set; }
        public IFormFile? Image { get; set; }
        public List<ListGenresModel> AllGenres { get; set; }
        public List<ListActorsModel> AllActors { get; set; }
    }
}
