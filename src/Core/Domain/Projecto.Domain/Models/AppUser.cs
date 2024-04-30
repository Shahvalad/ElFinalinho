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
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsBanned { get; set; }

        // Navigation properties
        public int? DisplayedPostId { get; set; }
        public CommunityPost? DisplayedPost { get; set; }
        
        public List<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<UserFavouriteGame> UserFavouriteGames { get; set; } = new List<UserFavouriteGame>();
        public AppUserProfilePicture? ProfilePicture { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>(); 
        public List<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
        public List<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<CommunityPost> Posts { get; set; } = new List<CommunityPost>();
        public List<UserLikesPost> LikedPosts { get; set; } = new List<UserLikesPost>();
        public List<UserTarotCard> UserTarotCards { get; set; } = new List<UserTarotCard>();
    }
}
