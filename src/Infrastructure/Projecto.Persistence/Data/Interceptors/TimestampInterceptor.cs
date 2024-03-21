using static Projecto.Persistence.Helpers.TimestampHelper;

namespace Projecto.Persistence.Data.Interceptors
{
    public class TimestampInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context?.ChangeTracker != null)
            {
                SetTimestamps(eventData.Context.ChangeTracker.Entries<BaseAuditableEntity>());
            }
            return result;
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context?.ChangeTracker != null)
            {
                SetTimestamps(eventData.Context.ChangeTracker.Entries<BaseAuditableEntity>());
            }
            return new ValueTask<InterceptionResult<int>>(result);
        }

    }
}
