using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.BusinessLogic.Implementation.Account.Validations;
using MovieCity.BusinessLogic.Implementation.GenreImp;
using MovieCity.BusinessLogic.Implementation.MovieImp;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.BusinessLogic.Implementation.ReviewImp;
using MovieCity.Common.DTOs;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;
using MovieCity.Entities.Enums;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieCity.BusinessLogic.Implementation.Account
{
    public class UserAccountService : BaseService
    {
        private readonly RegisterUserValidator registerUserValidator;
        private readonly EditUserValidator editUserValidator;
        private readonly ReviewService reviewService;
        private readonly MovieService movieService;
        private readonly GenreService genreService;
        private readonly IConfiguration _configuration;

        public UserAccountService(ServiceDependencies dependencies, ReviewService reviewService, MovieService movieService, GenreService genreService, IConfiguration configuration)
            : base(dependencies)
        {
            this.registerUserValidator = new RegisterUserValidator(UnitOfWork);
            this.editUserValidator = new EditUserValidator(UnitOfWork);
            this.reviewService = reviewService;
            this.movieService = movieService;
            this.genreService = genreService;
            this._configuration = configuration;
        }

        private string HashPassword(string password, string salt)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public async Task RegisterNewUser(RegisterModel model)
        {
            registerUserValidator.Validate(model).ThenThrow(model);

            var user = Mapper.Map<User>(model);

            var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
            user.Password = HashPassword(model.Password, salt);
            user.Salt = salt;

            user.UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = (int)RoleTypes.User}
            };

            UnitOfWork.Users.Insert(user);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<LoggedUserDTO> Login(string username, string password)
        {
            var user = await UnitOfWork.Users.Get()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserImage)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return new LoggedUserDTO { IsAuthenticated = false };
            }

            if (user.IsDeleted)
            {
                return new LoggedUserDTO { IsAuthenticated = false };
            }

            var passwordHash = HashPassword(password, user.Salt);

            if (user.Password != passwordHash)
            {
                return new LoggedUserDTO { IsAuthenticated = false };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach(var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            CurrentUser.Id = user.Id;
            CurrentUser.FullName = user.FullName;
            CurrentUser.Username = user.Username;
            CurrentUser.Email = user.Email;
            CurrentUser.IsAuthenticated = true;
            CurrentUser.Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            CurrentUser.Token = tokenHandler.WriteToken(token).ToString();

            return new LoggedUserDTO
            {
                Username = user.Username,
                Email = user.Email,
                IsAuthenticated = true,
                Image = GetCurrentUserImage(),
                Token = tokenHandler.WriteToken(token).ToString()
            };
        }

        public string GetCurrentUserImage()
        {
            var user =  UnitOfWork.Users.Get()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserImage)
                .FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (user == null)
            {
                throw new NotFoundErrorException();
            }

            if (user.UserImage != null)
                return string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(user.UserImage.Image));
            else
            {
                return null;
            }
        }

        public async Task<List<ViewUserItem>> GetUsers()
        {
            var users = await Mapper.ProjectTo<ViewUserItem>(UnitOfWork.Users.Get()).OrderBy(m => m.IsDeleted).ThenBy(m => m.Username).ToListAsync();

            foreach(var user in users)
            {
                user.IsAdmin = UnitOfWork.UserRoles.Get().Any(r => r.UserId == user.Id && r.RoleId == (int)RoleTypes.Admin);
            }

            return users;
        }

        public async Task<bool> ActivateUser(Guid id)
        {
            var user = await UnitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return false;

            user.IsDeleted = false;

            UnitOfWork.Users.Update(user);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(string id, string password)
        {
            if (id != null)
            {
                var GuidId = Guid.Parse(id);
                var user = await UnitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Id == GuidId);

                if (user == null) return false;
                if (IsLastAdmin(user.Id)) return false;

                user.IsDeleted = true;

                UnitOfWork.Users.Update(user);
            }
            else
            {
                var user = await UnitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Id == CurrentUser.Id);

                if (user == null) return false;

                if (user.Password != HashPassword(password, user.Salt))
                    return false;

                if (IsLastAdmin(user.Id)) return false;

                user.IsDeleted = true;

                UnitOfWork.Users.Update(user);
            }

            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public bool IsLastAdmin(Guid id)
        {
            if(CurrentUser.Id == id && CurrentUser.Roles.Contains("Admin"))
            {
                var adminNo = UnitOfWork.UserRoles.Get().Where(r => r.RoleId == ((int)RoleTypes.Admin)).Count();

                return adminNo == 1;
            }
            else
            {
                return false;
            }
        }

        public async Task<ViewUserProfileModel> GetUserProfileModelById(Guid id)
        {
            var user = await UnitOfWork.Users.Get()
                .Include(u => u.UserImage)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<ViewUserProfileModel>(user);

            if (user.UserImage != null)
            {
                model.ActualImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(user.UserImage.Image));
            }
            else
            {
                model.ActualImage = GetDefaultUserImage();
            }

            model.Status = await GetStatus(CurrentUser.Id, model.Id);
            model.RecentLikedMovies = await movieService.GetRecentLikedMovies(id);
            model.RecentReviews = await reviewService.GetRecentReviews(id);
            model.HasMoreThan3Reviews = user.Reviews.Count() > 3;
            model.FavoriteGenres = await genreService.GetFavoriteGenres(id);

            return model;
        }

        public async Task<ViewUserProfileModel> GetUserProfileModelByUsername(string username)
        {
            var user = await UnitOfWork.Users.Get()
                .Include(u => u.UserImage)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                throw new NotFoundErrorException();

            var model = Mapper.Map<ViewUserProfileModel>(user);

            if (user.UserImage != null)
            {
                model.ActualImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(user.UserImage.Image));
            }
            else
            {
                model.ActualImage = GetDefaultUserImage();
            }

            model.Status = await GetStatus(CurrentUser.Id, model.Id);
            model.RecentLikedMovies = await movieService.GetRecentLikedMovies(model.Id);
            model.RecentReviews = await reviewService.GetRecentReviews(model.Id);
            model.HasMoreThan3Reviews = user.Reviews.Count() > 3;
            model.FavoriteGenres = await genreService.GetFavoriteGenres(model.Id);

            return model;
        }

        private async Task<FriendStatusTypes> GetStatus(Guid currentUserId, Guid userId)
        {
            var friend = await UnitOfWork.Friends.Get().FirstOrDefaultAsync(s => (s.User1Id == currentUserId && s.User2Id == userId) || (s.User2Id == currentUserId && s.User1Id == userId));
            if (friend != null)
            {
                return FriendStatusTypes.Friends;
            }
            else
            {
                var sentFriendRequest = await UnitOfWork.FriendRequests.Get().FirstOrDefaultAsync(s => s.SenderId == currentUserId && s.ReceiverId == userId);
                if (sentFriendRequest != null)
                {
                    return FriendStatusTypes.SendedRequest;
                }
                else
                {
                    var receivedFriendRequest = await UnitOfWork.FriendRequests.Get().FirstOrDefaultAsync(s => s.ReceiverId == currentUserId && s.SenderId == userId);
                    if (receivedFriendRequest != null)
                    {
                        return FriendStatusTypes.ReceivedRequest;
                    }
                    else
                    {
                        return FriendStatusTypes.Default;
                    }
                }
            }
        }

        public async Task<CurrentUserDTO> UpdateUser(ViewUserProfileModel model)
        {
            editUserValidator.Validate(model).ThenThrow(model);

            var userToUpdate = await UnitOfWork.Users.Get()
                .Include(a => a.UserImage)
                .Include(a => a.UserRoles)
                    .ThenInclude(a => a.Role)
                .FirstOrDefaultAsync(g => g.Id == model.Id);

            if (userToUpdate == null) return null;

            userToUpdate.FirstName = model.FirstName;
            userToUpdate.LastName = model.LastName;
            userToUpdate.BirthDate = model.BirthDate;
            userToUpdate.Email = model.Email;
            userToUpdate.Username = model.Username;

            if (model.NewImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.NewImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    if (userToUpdate.UserImage != null)
                    {
                        userToUpdate.UserImage.Image = fileBytes;
                    }
                    else
                    {
                        userToUpdate.UserImage = new UserImage
                        {
                            Image = fileBytes
                        };
                    }
                }
            }
            else
            {
                if (model.DeleteActualImage)
                {
                    userToUpdate.UserImage = null;
                }
            }

            UnitOfWork.Users.Update(userToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return new CurrentUserDTO
            {
                Id = userToUpdate.Id,
                FullName = userToUpdate.FullName,
                Username = userToUpdate.Username,
                Email = userToUpdate.Email,
                IsAuthenticated = true,
                Roles = userToUpdate.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task<bool> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var user = await UnitOfWork.Users.Get()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == CurrentUser.Id);

            if (user == null)
            {
                throw new NotFoundErrorException();
            }

            var passwordHash = HashPassword(currentPassword, user.Salt);

            if (user.Password != passwordHash || newPassword != confirmNewPassword)
            {
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            if(!(hasNumber.IsMatch(newPassword) && hasUpperChar.IsMatch(newPassword) && hasMinimum8Chars.IsMatch(newPassword)))
            {
                return false;
            }

            var newPasswordHash = HashPassword(newPassword, user.Salt);
            user.Password = newPasswordHash;

            UnitOfWork.Users.Update(user);
            await UnitOfWork.SaveChangesAsync();

            return true;
        }

        public string GetDefaultUserImage()
        {
            var image = "";
            using (var fs = new FileStream("C:\\Users\\stefania.gutu\\Desktop\\Academie\\Proiect - MovieCity\\StefaniaGutu\\src\\MovieCity.WebApp\\wwwroot\\no-profile-picture.png", FileMode.Open, FileAccess.Read))
            {
                byte[] userImage = new byte[fs.Length];
                fs.Read(userImage, 0, Convert.ToInt32(fs.Length));
                image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(userImage));
            }

            return image;
        }
    }
}
