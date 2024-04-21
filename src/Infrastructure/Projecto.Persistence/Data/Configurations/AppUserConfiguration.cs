using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Persistence.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(u => u.SentFriendRequests)
                .WithOne(f => f.Requester)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasMany(u => u.ReceivedFriendRequests)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
