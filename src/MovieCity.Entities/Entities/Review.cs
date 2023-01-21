using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Review : IEntity
    {
        public Review()
        {
            Posts = new HashSet<Post>();
        }

        public Guid Id { get; set; }
        public Guid MovieSeriesId { get; set; }
        public Guid UserId { get; set; }
        public string ReviewText { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public bool ShowInFeed { get; set; }

        public virtual MoviesAndSeries MovieSeries { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; }
    }
}
