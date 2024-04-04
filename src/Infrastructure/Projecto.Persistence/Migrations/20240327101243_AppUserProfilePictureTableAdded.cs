using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppUserProfilePictureTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCoverImage",
                table: "PublisherImages");

            migrationBuilder.DropColumn(
                name: "IsCoverImage",
                table: "DeveloperImages");

            migrationBuilder.CreateTable(
                name: "AppUserProfilePictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProfilePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserProfilePictures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProfilePictures_UserId",
                table: "AppUserProfilePictures",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserProfilePictures");

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverImage",
                table: "PublisherImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverImage",
                table: "DeveloperImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
