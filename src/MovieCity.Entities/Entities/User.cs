using MovieCity.Common;
using System;
using System.Collections.Generic;

namespace MovieCity.Entities
{
    public partial class User : IEntity
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FriendRequestReceivers = new HashSet<FriendRequest>();
            FriendRequestSenders = new HashSet<FriendRequest>();
            LikeMovies = new HashSet<LikeMovie>();
            LikePosts = new HashSet<LikePost>();
            Posts = new HashSet<Post>();
            Reviews = new HashSet<Review>();
            Watches = new HashSet<Watch>();
            UserRoles = new HashSet<UserRole>();
            User1Ids = new HashSet<Friend>();
            User2Ids = new HashSet<Friend>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

        public virtual UserImage UserImage { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestReceivers { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestSenders { get; set; }
        public virtual ICollection<Friend> User1Ids { get; set; }
        public virtual ICollection<Friend> User2Ids { get; set; }
        public virtual ICollection<LikeMovie> LikeMovies { get; set; }
        public virtual ICollection<LikePost> LikePosts { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
