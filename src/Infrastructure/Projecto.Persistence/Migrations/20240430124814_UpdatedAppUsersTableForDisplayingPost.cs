using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppUsersTableForDisplayingPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityPosts_CommunityPosts_DisplayedPostId",
                table: "CommunityPosts");

            migrationBuilder.DropIndex(
                name: "IX_CommunityPosts_DisplayedPostId",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "DisplayedPostId",
                table: "CommunityPosts");

            migrationBuilder.AddColumn<int>(
                name: "DisplayedPostId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DisplayedPostId",
                table: "AspNetUsers",
                column: "DisplayedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CommunityPosts_DisplayedPostId",
                table: "AspNetUsers",
                column: "DisplayedPostId",
                principalTable: "CommunityPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CommunityPosts_DisplayedPostId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DisplayedPostId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayedPostId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DisplayedPostId",
                table: "CommunityPosts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPosts_DisplayedPostId",
                table: "CommunityPosts",
                column: "DisplayedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityPosts_CommunityPosts_DisplayedPostId",
                table: "CommunityPosts",
                column: "DisplayedPostId",
                principalTable: "CommunityPosts",
                principalColumn: "Id");
        }
    }
}
