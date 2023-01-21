using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Actor : IEntity
    {
        public Actor()
        {
            Movies = new HashSet<MoviesAndSeries>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ActorImage ActorImage { get; set; } = null!;

        public virtual ICollection<MoviesAndSeries> Movies { get; set; }
    }
}
