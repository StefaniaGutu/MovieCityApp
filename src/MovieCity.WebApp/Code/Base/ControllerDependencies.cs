using MovieCity.Common.DTOs;

namespace MovieCity.WebApp.Code.Base
{
    public class ControllerDependencies
    {
        public CurrentUserDTO CurrentUser { get; set; }

        public ControllerDependencies(CurrentUserDTO currentUser)
        {
            this.CurrentUser = currentUser;
        }
    }
}
