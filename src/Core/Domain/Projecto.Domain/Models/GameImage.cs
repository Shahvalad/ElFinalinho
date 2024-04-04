namespace Projecto.Domain.Models
{
    public class GameImage : BaseImage
    {
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public bool IsCoverImage { get; set; } = false;
    }
}
