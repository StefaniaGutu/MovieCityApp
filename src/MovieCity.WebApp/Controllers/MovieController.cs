using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.ActorImp;
using MovieCity.BusinessLogic.Implementation.GenreImp;
using MovieCity.BusinessLogic.Implementation.MovieImp;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using MovieCity.Entities.Enums;
using MovieCity.WebApp.Code.Base;
using X.PagedList;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : BaseController
    {
        private readonly MovieService Service;
        private readonly ActorService actorService;
        private readonly GenreService genreService;

        public MovieController(ControllerDependencies dependencies, MovieService service, ActorService actorService, GenreService genreService)
            : base(dependencies)
        {
            this.Service = service;
            this.actorService = actorService;
            this.genreService = genreService;
        }

        [HttpGet]
        [Route("getAllMovies")]
        [Authorize]
        public async Task<IActionResult> Movies()
        {
            var model = await Service.GetMoviesWithDetails();

            return NotFound(model);

            return Ok(model);
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> Watchlist([FromQuery] string searchString, string currentFilter, int? page)
        //{
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    @ViewBag.CurrentFilter = searchString;

        //    int pageSize = 6;
        //    int pageNumber = (page ?? 1);

        //    var model = await Service.GetWatchedMovies(false, searchString, pageNumber, pageSize);

        //    return View("Watchlist", model);
        //}

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> WatchedMovies([FromQuery] string searchString, string currentFilter, int? page)
        //{
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    @ViewBag.CurrentFilter = searchString;

        //    int pageSize = 6;
        //    int pageNumber = (page ?? 1);

        //    var model = await Service.GetWatchedMovies(true, searchString, pageNumber, pageSize);

        //    return View("WatchedMovies", model);
        //}

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> LikedMovies([FromQuery] string searchString, string currentFilter, int? page)
        //{
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    @ViewBag.CurrentFilter = searchString;

        //    int pageSize = 6;
        //    int pageNumber = (page ?? 1);

        //    var model = await Service.GetLikedMovies(searchString, pageNumber, pageSize);

        //    return View("LikedMovies", model);
        //}

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> RecommendedMovies()
        //{
        //    var model = await Service.GetRecommendedMovies();

        //    return View("RecommendedMovies", model);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ViewMovieDetails(Guid id)
        //{
        //    var model = await Service.GetMovieDetailsModelById(id);

        //    return View(model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> MovieDetails(Guid id)
        //{
        //    var model = await Service.GetMovieModelById(id);

        //    return View("MovieDetails", model);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ViewMovies([FromQuery] string searchString, string currentFilter, int? page)
        //{
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    @ViewBag.CurrentFilter = searchString != null ? searchString : "";

        //    int pageSize = 5;
        //    int pageNumber = (page ?? 1);
            
        //    var model = await Service.GetMovies(searchString, pageNumber, pageSize);

        //    return View(model);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateMovie()
        //{
        //    var model = new CreateMovieModel();
        //    model.AllActors = await actorService.GetAllActors();
        //    model.AllGenres = await genreService.GetGenres();

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateMovie(CreateMovieModel model)
        //{
        //    await Service.CreateNewMovie(model);

        //    return RedirectToAction("ViewMovies", "Movie");
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> EditMovie(Guid id)
        //{
        //    var model = await Service.GetEditMovieModelById(id);

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> EditMovie(EditMovieModel model)
        //{
        //    if (await Service.UpdateMovie(model))
        //    {
        //        return RedirectToAction("ViewMovies", "Movie");
        //    }
        //    else
        //    {
        //        return View(model);
        //    }  
        //}

        //[HttpDelete]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteMovie(string id)
        //{
        //    if (await Service.DeleteMovie(id))
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }   
        //}
    }
}
