using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Persistence.Data.Configurations
{
    public class UserLikesPostConfiguration : IEntityTypeConfiguration<UserLikesPost>
    {
        public void Configure(EntityTypeBuilder<UserLikesPost> builder)
        {
            builder.HasKey(ulp => new { ulp.UserId, ulp.PostId });

            builder.HasOne(ulp => ulp.User)
                .WithMany(u => u.LikedPosts)
                .HasForeignKey(ulp => ulp.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Disable cascading delete

            builder.HasOne(ulp => ulp.Post)
                .WithMany(p => p.LikedByUsers)
                .HasForeignKey(ulp => ulp.PostId)
                .OnDelete(DeleteBehavior.NoAction); // Disable cascading delete
        }
    }


}
