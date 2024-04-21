namespace Projecto.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public DateTime MemberSince { get; set; }
        public decimal Balance { get;  set; } 
        public decimal TotalSpendings { get;  set; }
        
        // Navigation properties
        public List<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<UserFavouriteGame> UserFavouriteGames { get; set; } = new List<UserFavouriteGame>();
        public AppUserProfilePicture? ProfilePicture { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>(); 
        public List<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
        public List<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
