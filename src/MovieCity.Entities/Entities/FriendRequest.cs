using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class FriendRequest : IEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime Date { get; set; }

        public virtual User Receiver { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
