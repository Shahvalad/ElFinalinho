using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserFavouriteGamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavouriteGame_AspNetUsers_UserId",
                table: "UserFavouriteGame");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavouriteGame_Games_GameId",
                table: "UserFavouriteGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavouriteGame",
                table: "UserFavouriteGame");

            migrationBuilder.RenameTable(
                name: "UserFavouriteGame",
                newName: "UserFavouriteGames");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavouriteGame_GameId",
                table: "UserFavouriteGames",
                newName: "IX_UserFavouriteGames_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavouriteGames",
                table: "UserFavouriteGames",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavouriteGames_AspNetUsers_UserId",
                table: "UserFavouriteGames",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavouriteGames_Games_GameId",
                table: "UserFavouriteGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavouriteGames_AspNetUsers_UserId",
                table: "UserFavouriteGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavouriteGames_Games_GameId",
                table: "UserFavouriteGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavouriteGames",
                table: "UserFavouriteGames");

            migrationBuilder.RenameTable(
                name: "UserFavouriteGames",
                newName: "UserFavouriteGame");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavouriteGames_GameId",
                table: "UserFavouriteGame",
                newName: "IX_UserFavouriteGame_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavouriteGame",
                table: "UserFavouriteGame",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavouriteGame_AspNetUsers_UserId",
                table: "UserFavouriteGame",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavouriteGame_Games_GameId",
                table: "UserFavouriteGame",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
