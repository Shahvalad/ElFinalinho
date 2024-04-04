namespace Projecto.Domain.Models
{
    public class DeveloperImage : BaseImage
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; } = null!;
    }
}
