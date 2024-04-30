using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Persistence.Data.Configurations
{
    public class CommunityPostConfiguration : IEntityTypeConfiguration<CommunityPost>
    {
        public void Configure(EntityTypeBuilder<CommunityPost> builder)
        {
            builder
                .HasOne(cp => cp.CommunityPostImage)
                .WithOne(cpi => cpi.CommunityPost)
                .HasForeignKey<CommunityPostImage>(cpi => cpi.CommunityPostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
