namespace Projecto.Domain.Models
{
    public class Community : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public int Threads { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public List<CommunityPost> Posts { get; set; } = new();
        public CommunityImage Image { get; set; }
    }
}
