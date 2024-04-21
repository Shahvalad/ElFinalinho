using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Persistence.Data.Configurations
{
    public class UserFavouriteGameConfiguration : IEntityTypeConfiguration<UserFavouriteGame>
    {
        public void Configure(EntityTypeBuilder<UserFavouriteGame> builder)
        {
            builder.ToTable("UserFavouriteGames");

            builder.HasKey(ufg => new { ufg.UserId, ufg.GameId });

            builder.HasOne(ufg => ufg.User)
                .WithMany(u => u.UserFavouriteGames)
                .HasForeignKey(ufg => ufg.UserId);

            builder.HasOne(ufg => ufg.Game)
                .WithMany(g => g.UserFavouriteGames)
                .HasForeignKey(ufg => ufg.GameId);
        }
    }
}
