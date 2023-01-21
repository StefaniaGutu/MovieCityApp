using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.Common.DTOs;
using MovieCity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Models
{
    public class CreateMovieModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Guid>? Genres { get; set; }
        public List<Guid>? Actors { get; set; }
        public IFormFile? Image { get; set; }
        public List<ListGenresModel> AllGenres { get; set; }
        public List<ListActorsModel> AllActors { get; set; }
    }
}
