using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Movie : IEntity
    {
        public Guid Id { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual MoviesAndSeries IdNavigation { get; set; } = null!;
    }
}
