namespace Projecto.Domain.Models
{
    public class Genre : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}
