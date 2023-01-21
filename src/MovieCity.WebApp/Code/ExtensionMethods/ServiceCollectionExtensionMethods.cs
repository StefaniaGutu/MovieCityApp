using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.Account;
using MovieCity.BusinessLogic.Implementation.ActorImp;
using MovieCity.BusinessLogic.Implementation.EpisodeImp;
using MovieCity.BusinessLogic.Implementation.FeedImp;
using MovieCity.BusinessLogic.Implementation.FriendImp;
using MovieCity.BusinessLogic.Implementation.GenreImp;
using MovieCity.BusinessLogic.Implementation.MovieImp;
using MovieCity.BusinessLogic.Implementation.ReviewImp;
using MovieCity.BusinessLogic.Implementation.SeasonImp;
using MovieCity.BusinessLogic.Implementation.SeriesImp;
using MovieCity.BusinessLogic.Implementation.WatchAndLikeImp;
using MovieCity.Common.DTOs;
using MovieCity.WebApp.Code.Base;

namespace MovieCity.WebApp.Code.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();

            return services;
        }

        public static IServiceCollection AddMovieCityBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<UserAccountService>();
            services.AddScoped<MovieService>();
            services.AddScoped<GenreService>();
            services.AddScoped<ActorService>();
            services.AddScoped<SeriesService>();
            services.AddScoped<SeasonService>();
            services.AddScoped<EpisodeService>();
            services.AddScoped<WatchAndLikeService>();
            services.AddScoped<ReviewService>();
            services.AddScoped<FriendService>();
            services.AddScoped<FeedService>();

            return services;
        }

        public static IServiceCollection AddMovieCityCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor?.HttpContext;
                var claims = httpContext?.User.Claims;

                var userIdClaim = claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
                var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);

                //var userAccountService = s.GetService<UserAccountService>();
                return new CurrentUserDTO
                {
                    Id = id,
                    IsAuthenticated = httpContext?.User?.Identity?.IsAuthenticated ?? false,
                    Username = httpContext?.User?.Identity?.Name ?? "",
                    Roles = httpContext?.User?.Claims.Where(s => s.Value == "Admin" || s.Value == "User").Select(s => s.Value).ToList()
                    //Image = userAccountService.GetCurrentUserImage()
                };
            });

            return services;
        }
    }
}
