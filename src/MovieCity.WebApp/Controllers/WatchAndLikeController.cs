using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.WatchAndLikeImp;
using MovieCity.Common.DTOs;
using MovieCity.WebApp.Code.Base;
using System.Security.Claims;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WatchAndLikeController : BaseController
    {
        private readonly WatchAndLikeService Service;

        public WatchAndLikeController(ControllerDependencies dependencies, WatchAndLikeService service) : base(dependencies)
        {
            this.Service = service;
        }

        [HttpPost]
        [Route("addWatch")]
        [Authorize]
        public async Task<IActionResult> AddWatch(Guid movieId, bool isAlreadyWatched)
        {
            await Service.AddWatch(movieId, isAlreadyWatched);

            return Ok();
        }

        [HttpPost]
        [Route("addLike")]
        [Authorize]
        public async Task<IActionResult> AddLike(Guid movieId, bool isLiked)
        {
            await Service.AddLike(movieId, isLiked);

            return Ok();
        }
    }
}
