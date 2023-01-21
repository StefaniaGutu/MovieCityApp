using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class MoviesAndSeries : IEntity
    {
        public MoviesAndSeries()
        {
            LikeMovies = new HashSet<LikeMovie>();
            Reviews = new HashSet<Review>();
            Seasons = new HashSet<Season>();
            Watches = new HashSet<Watch>();
            Actors = new HashSet<Actor>();
            Genres = new HashSet<Genre>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsSeries { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual MovieOrSeriesImage MovieOrSeriesImage { get; set; } = null!;
        public virtual ICollection<LikeMovie> LikeMovies { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
