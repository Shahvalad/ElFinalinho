namespace Projecto.Domain.Models
{
    public class UserGame
    {
        public string UserId { get; set; }
        public AppUser User { get; set; } 

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
