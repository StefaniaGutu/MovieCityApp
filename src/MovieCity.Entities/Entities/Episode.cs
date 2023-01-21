using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Episode : IEntity
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public string Name { get; set; } = null!;
        public int Duration { get; set; }
        public int EpisodeNo { get; set; }

        public virtual Season Season { get; set; } = null!;
    }
}
