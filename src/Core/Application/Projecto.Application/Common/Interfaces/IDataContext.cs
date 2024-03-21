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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void MarkAsModified(object entity);
        void MarkAsCreated(object entity);

    }
}
