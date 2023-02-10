using Microsoft.AspNetCore.Mvc;
using MovieCity.Common.DTOs;

namespace MovieCity.WebApp.Code.Base
{
    public class BaseController : ControllerBase
    {
        public CurrentUserDTO CurrentUser;

        public BaseController(ControllerDependencies dependencies) : base()
        {
            CurrentUser = dependencies.CurrentUser;
        }
    }
}
