namespace Projecto.Persistence.Helpers
{
    public static class TimestampHelper
    {
        public static void SetTimestamps(IEnumerable<EntityEntry<BaseAuditableEntity>> entities)
        {
            foreach (var entityEntry in entities)
            {
                var entity = entityEntry.Entity;

                _ = entityEntry.State switch
                {
                    EntityState.Added => entity.CreatedAt = DateTime.UtcNow,
                    EntityState.Modified => entity.UpdatedAt = DateTime.UtcNow,
                    EntityState.Deleted => entity.DeletedAt = DateTime.UtcNow,
                    _ => throw new ArgumentOutOfRangeException(nameof(entityEntry.State))
                };
            }
        }
    }
}
