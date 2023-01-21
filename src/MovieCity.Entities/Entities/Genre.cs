using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Genre : IEntity
    {
        public Genre()
        {
            Movies = new HashSet<MoviesAndSeries>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<MoviesAndSeries> Movies { get; set; }
    }
}
