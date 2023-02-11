using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.MovieImp;
using MovieCity.WebApp.Code.Base;
using System.Diagnostics;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : BaseController
    {
        private readonly MovieService movieService;

        public HomeController(ControllerDependencies dependencies, MovieService movieService) : base(dependencies)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        [Route("getPopularAndLikedMovies")]
        public async Task<IActionResult> Index()
        {
            var model = await movieService.GetPopularAndLikedMovies();
            return Ok(model);
        }
    }
}