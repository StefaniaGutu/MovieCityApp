using MovieCity.Common;
using MovieCity.Entities;
using MovieCity.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.DataAccess
{
    public class UnitOfWork
    {
        private readonly MovieAppDBContext Context;

        public UnitOfWork(MovieAppDBContext context)
        {
            this.Context = context;
        }

        private IRepository<User> users;
        public IRepository<User> Users => users ?? (users = new BaseRepository<User>(Context));

        private IRepository<MoviesAndSeries> moviesAndSeries;
        public IRepository<MoviesAndSeries> MoviesAndSeries => moviesAndSeries ?? (moviesAndSeries = new BaseRepository<MoviesAndSeries>(Context));

        private IRepository<Movie> movies;
        public IRepository<Movie> Movies => movies ?? (movies = new BaseRepository<Movie>(Context));

        private IRepository<Genre> genres;
        public IRepository<Genre> Genres => genres ?? (genres = new BaseRepository<Genre>(Context));

        private IRepository<Actor> actors;
        public IRepository<Actor> Actors => actors ?? (actors = new BaseRepository<Actor>(Context));

        private IRepository<Season> seasons;
        public IRepository<Season> Seasons => seasons ?? (seasons = new BaseRepository<Season>(Context));

        private IRepository<Episode> episodes;
        public IRepository<Episode> Episodes => episodes ?? (episodes = new BaseRepository<Episode>(Context));

        private IRepository<Watch> watch;
        public IRepository<Watch> Watch => watch ?? (watch = new BaseRepository<Watch>(Context));

        private IRepository<LikeMovie> likeMovie;
        public IRepository<LikeMovie> LikeMovie => likeMovie ?? (likeMovie = new BaseRepository<LikeMovie>(Context));

        private IRepository<Review> reviews;
        public IRepository<Review> Reviews => reviews ?? (reviews = new BaseRepository<Review>(Context));

        private IRepository<Post> posts;
        public IRepository<Post> Posts => posts ?? (posts = new BaseRepository<Post>(Context));

        private IRepository<Comment> comments;
        public IRepository<Comment> Comments => comments ?? (comments = new BaseRepository<Comment>(Context));

        private IRepository<LikePost> likePost;
        public IRepository<LikePost> LikePost => likePost ?? (likePost = new BaseRepository<LikePost>(Context));

        private IRepository<FriendRequest> friendRequests;
        public IRepository<FriendRequest> FriendRequests => friendRequests ?? (friendRequests = new BaseRepository<FriendRequest>(Context));

        private IRepository<Friend> friends;
        public IRepository<Friend> Friends => friends ?? (friends = new BaseRepository<Friend>(Context));

        private IRepository<UserImage> userImages;
        public IRepository<UserImage> UserImages => userImages ?? (userImages = new BaseRepository<UserImage>(Context));

        private IRepository<UserRole> userRoles;
        public IRepository<UserRole> UserRoles => userRoles ?? (userRoles = new BaseRepository<UserRole>(Context));

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
