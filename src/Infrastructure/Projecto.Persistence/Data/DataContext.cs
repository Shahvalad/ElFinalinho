namespace Projecto.Persistence.Data
{
    public class DataContext : IdentityDbContext<AppUser>, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Game> Games { get; set; } 
        public DbSet<Genre> Genres { get; set; } 
        public DbSet<GameGenre> GameGenres { get; set; } 
        public DbSet<UserGame> UserGames { get; set; } 
        public DbSet<Developer> Developers { get; set; } 
        public DbSet<Publisher> Publishers { get; set; } 
        public DbSet<PublisherImage> PublisherImages { get; set; } 
        public DbSet<GameImage> GameImages { get; set; } 
        public DbSet<GameKey> GameKeys { get; set; } 
        public DbSet<DeveloperImage> DeveloperImages { get; set; } 
        public DbSet<AppUserProfilePicture> AppUserProfilePictures { get; set; } 
        public DbSet<UserFavouriteGame> UserFavouriteGames { get; set; } 
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<Friendship> Friendships { get; set; } 
        public DbSet<Message> Messages { get; set; } 
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityImage> CommunityImages { get; set; }
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<CommunityPostImage> CommunityPostImages { get; set; }
        public DbSet<UserLikesPost> UserLikesPosts { get; set; }
        public DbSet<TarotCard> TarotCards { get; set; }
        public DbSet<UserTarotCard> UserTarotCards { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<MarketItem> MarketItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        public new ChangeTracker ChangeTracker => base.ChangeTracker;
        public new EntityEntry Entry(object entity) => base.Entry(entity);

        public void MarkAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void MarkAsCreated(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }
    }
}
