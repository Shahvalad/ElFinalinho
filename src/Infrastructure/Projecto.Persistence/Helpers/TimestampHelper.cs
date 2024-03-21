namespace Projecto.Persistence.Helpers
{
    public static class TimestampHelper
    {
        public static void SetTimestamps(IEnumerable<EntityEntry<BaseAuditableEntity>> entities)
        {
            foreach (var entityEntry in entities)
            {
                var entity = entityEntry.Entity;

                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
