namespace Projecto.Persistence.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<GameGenre> GameGenres { get; set; } = default!;
        public DbSet<Developer> Developer { get; set; } = default!;
        public DbSet<Publisher> Publisher { get; set; } = default!;

        public DbSet<PublisherImage> PublisherImages { get; set; } = default!;
        public DbSet<GameImage> GameImages { get; set; } = default!;
        public DbSet<DeveloperImage> DeveloperImages { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public void MarkAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

    }
}
