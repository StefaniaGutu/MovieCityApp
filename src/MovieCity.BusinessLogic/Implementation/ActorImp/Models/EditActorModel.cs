using Microsoft.AspNetCore.Http;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Models
{
    public class EditActorModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? NewImage { get; set; }
        public string ActualImage { get; set; }
        public bool HasAvailableImage { get; set; }
        public bool DeleteActualImage { get; set; }
    }
}
