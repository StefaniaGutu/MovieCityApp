using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class ActorImage : IEntity
    {
        public Guid ActorId { get; set; }
        public byte[] Image { get; set; } = null!;

        public virtual Actor Actor { get; set; } = null!;
    }
}
