using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class LikePost : IEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool IsLiked { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
