using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCity.BusinessLogic.Implementation.Account;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.Common.DTOs;
using MovieCity.WebApp.Code.Base;

namespace MovieCity.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountController : BaseController
    {
        private readonly UserAccountService Service;

        public UserAccountController(ControllerDependencies dependencies, UserAccountService service) : base(dependencies)
        {
            this.Service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            await Service.RegisterNewUser(model);
            var user = await Service.Login(model.Username, model.Password);

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)

        {
            var user = await Service.Login(model.Username, model.Password);

            if (!user.IsAuthenticated)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> UserProfile(Guid id)
        {
            var model = await Service.GetUserProfileModelById(id);

            return Ok(model);
        }
    }
}
