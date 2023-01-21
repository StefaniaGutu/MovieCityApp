using Microsoft.AspNetCore.Http;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Models
{
    public class CreateActorModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
