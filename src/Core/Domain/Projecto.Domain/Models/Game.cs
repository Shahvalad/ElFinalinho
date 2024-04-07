namespace Projecto.Domain.Models
{
    //TODO : Add Relase date for game!
    public class Game : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Range(5, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 5.")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        public int StockCount { get; set; }
        public List<GameImage> Images { get; set; } = new List<GameImage>();
        public List<GameKey> GameKeys { get; set; } = new List<GameKey>();

        //Navigation Properties 
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; } = null!;

        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

        public List<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<UserFavouriteGame> UserFavouriteGames { get; set; } = new List<UserFavouriteGame>();

    }
}
