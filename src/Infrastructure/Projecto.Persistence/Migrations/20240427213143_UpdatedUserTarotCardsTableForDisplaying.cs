using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserTarotCardsTableForDisplaying : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayedOnProfile",
                table: "UserTarotCards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplayedOnProfile",
                table: "UserTarotCards");
        }
    }
}
