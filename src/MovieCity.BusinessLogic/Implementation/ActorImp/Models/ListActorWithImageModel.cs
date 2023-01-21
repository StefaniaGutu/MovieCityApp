namespace MovieCity.BusinessLogic.Implementation.ActorImp.Models
{
    public class ListActorWithImageModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool HasAvailableImage { get; set; }
    }
}
