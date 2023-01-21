using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class MovieOrSeriesImage : IEntity
    {
        public Guid MovieSeriesId { get; set; }
        public byte[] Image { get; set; } = null!;

        public virtual MoviesAndSeries MovieSeries { get; set; } = null!;
    }
}
