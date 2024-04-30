public class UserTarotCardConfiguration : IEntityTypeConfiguration<UserTarotCard>
{
    public void Configure(EntityTypeBuilder<UserTarotCard> builder)
    {
        builder.HasKey(utc => utc.Id);

        builder.Property(utc => utc.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(utc => utc.User)
            .WithMany(u => u.UserTarotCards)
            .HasForeignKey(utc => utc.UserId);

        builder.HasOne(utc => utc.TarotCard)
            .WithMany(t => t.UserTarotCards)
            .HasForeignKey(utc => utc.TarotCardId);
    }
}