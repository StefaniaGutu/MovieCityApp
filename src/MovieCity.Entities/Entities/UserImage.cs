using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class UserImage : IEntity
    {
        public Guid UserId { get; set; }
        public byte[] Image { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
