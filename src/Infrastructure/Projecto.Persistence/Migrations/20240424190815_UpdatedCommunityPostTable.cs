using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommunityPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "CommunityPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "CommunityPosts");
        }
    }
}
