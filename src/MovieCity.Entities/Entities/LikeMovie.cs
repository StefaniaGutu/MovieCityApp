using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class LikeMovie : IEntity
    {
        public Guid UserId { get; set; }
        public Guid MovieSeriesId { get; set; }
        public bool IsLiked { get; set; }
        public DateTime? Date { get; set; }

        public virtual MoviesAndSeries MovieSeries { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
