namespace Projecto.Domain.Models
{
    public class Developer : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set;}

        public virtual ICollection<Game>? Games { get; set; } = new List<Game>();
        public DeveloperImage? Logo { get; set; }

    }
}
