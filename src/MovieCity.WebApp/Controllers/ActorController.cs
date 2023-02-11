using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.ActorImp;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.WebApp.Code.Base;
using X.PagedList;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : BaseController
    {
        private readonly ActorService Service;

        public ActorController(ControllerDependencies dependencies, ActorService service) 
            : base(dependencies)
        {
            this.Service = service;
        }

        [HttpGet]
        [Route("getActorDetails")]
        [Authorize]
        public async Task<IActionResult> ActorDetails(Guid id)
        {
            var model = await Service.GetActorDetailsModelById(id);

            return Ok(model);
        }
    }
}
