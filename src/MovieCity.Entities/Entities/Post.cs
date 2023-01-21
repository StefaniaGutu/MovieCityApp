using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class Post : IEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            LikePosts = new HashSet<LikePost>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ReviewId { get; set; }
        public string? PostText { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual Review Review { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikePost> LikePosts { get; set; }
    }
}
