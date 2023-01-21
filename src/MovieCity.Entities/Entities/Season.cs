using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Season : IEntity
    {
        public Season()
        {
            Episodes = new HashSet<Episode>();
        }

        public Guid Id { get; set; }
        public Guid MovieSeriesId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }

        public virtual MoviesAndSeries MovieSeries { get; set; } = null!;
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
