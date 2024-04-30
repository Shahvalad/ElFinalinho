using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Projecto.Application.Common.Interfaces
{
    public interface IDataContext
    {
        DbSet<Game> Games { get; }
        DbSet<Genre> Genres { get; }
        DbSet<GameGenre> GameGenres { get; }
        DbSet<Developer> Developers { get; }
        DbSet<Publisher> Publishers { get; }
        DbSet<PublisherImage> PublisherImages { get; }
        DbSet<GameImage> GameImages { get; }
        DbSet<DeveloperImage> DeveloperImages { get; }
        DbSet<UserGame> UserGames { get; }
        DbSet<GameKey> GameKeys { get; }
        DbSet<AppUserProfilePicture> AppUserProfilePictures { get; }
        DbSet<UserFavouriteGame> UserFavouriteGames { get; }
        DbSet<Review> Reviews { get; }
        DbSet<Friendship> Friendships { get; }
        DbSet<Message> Messages { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Community> Communities { get; }
        DbSet<CommunityImage> CommunityImages { get; }
        DbSet<CommunityPost> CommunityPosts { get; }
        DbSet<CommunityPostImage> CommunityPostImages { get; }
        DbSet<UserLikesPost> UserLikesPosts { get; }
        DbSet<TarotCard> TarotCards { get;}
        DbSet<UserTarotCard> UserTarotCards { get;}
        DbSet<MarketItem> MarketItems { get; }
        DbSet<Listing> Listings { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        ChangeTracker ChangeTracker { get; }
        EntityEntry Entry(object entity);


    }
}
