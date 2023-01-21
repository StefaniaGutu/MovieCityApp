using MovieCity.Common;

namespace MovieCity.Entities
{
    public partial class Friend : IEntity
    {
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }
        public DateTime Date { get; set; }

        public virtual User User1 { get; set; } = null!;
        public virtual User User2 { get; set; } = null!;
    }
}
