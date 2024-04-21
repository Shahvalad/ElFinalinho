namespace Projecto.Domain.Models
{
    public class UserFavouriteGame
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public AppUser User { get; set; }
        public Game Game { get; set; }
    }
}
