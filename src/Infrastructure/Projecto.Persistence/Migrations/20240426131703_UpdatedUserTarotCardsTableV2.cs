using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserTarotCardsTableV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTarotCards",
                table: "UserTarotCards");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserTarotCards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTarotCards",
                table: "UserTarotCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTarotCards_UserId",
                table: "UserTarotCards",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTarotCards",
                table: "UserTarotCards");

            migrationBuilder.DropIndex(
                name: "IX_UserTarotCards_UserId",
                table: "UserTarotCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTarotCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTarotCards",
                table: "UserTarotCards",
                columns: new[] { "UserId", "TarotCardId" });
        }
    }
}
