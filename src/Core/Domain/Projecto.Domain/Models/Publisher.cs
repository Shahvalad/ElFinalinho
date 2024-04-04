namespace Projecto.Domain.Models
{
    public class Publisher : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Game>? Games { get; set; } = new List<Game>();
        public PublisherImage? Logo { get; set; }
    }
}
