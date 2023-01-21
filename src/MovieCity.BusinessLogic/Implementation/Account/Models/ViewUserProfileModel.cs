using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.Entities.Enums;

namespace MovieCity.BusinessLogic.Implementation.Account.Models
{
    public class ViewUserProfileModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } 
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; } 
        public DateTime? BirthDate { get; set; }
        public FriendStatusTypes Status { get; set; }
        public IFormFile? NewImage { get; set; }
        public string ActualImage { get; set; }
        public bool HasAvailableImage { get; set; }
        public bool DeleteActualImage { get; set; }
        public List<string> FavoriteGenres { get; set; }
        public List<MovieWithGenresModel> RecentLikedMovies { get; set; }
        public List<MovieWithRatingModel> RecentReviews { get; set; }
        public bool HasMoreThan3Reviews { get; set; }
    }
}
