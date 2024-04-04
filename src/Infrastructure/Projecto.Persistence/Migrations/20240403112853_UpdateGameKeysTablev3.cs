using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameKeysTablev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameKey_Games_GameId",
                table: "GameKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameKey",
                table: "GameKey");

            migrationBuilder.RenameTable(
                name: "GameKey",
                newName: "GameKeys");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "GameKeys",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "GameKeys",
                newName: "IsAssigned");

            migrationBuilder.RenameIndex(
                name: "IX_GameKey_GameId",
                table: "GameKeys",
                newName: "IX_GameKeys_GameId");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "GameKeys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameKeys",
                table: "GameKeys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameKeys_Games_GameId",
                table: "GameKeys",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameKeys_Games_GameId",
                table: "GameKeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameKeys",
                table: "GameKeys");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "GameKeys");

            migrationBuilder.RenameTable(
                name: "GameKeys",
                newName: "GameKey");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "GameKey",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "IsAssigned",
                table: "GameKey",
                newName: "IsUsed");

            migrationBuilder.RenameIndex(
                name: "IX_GameKeys_GameId",
                table: "GameKey",
                newName: "IX_GameKey_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameKey",
                table: "GameKey",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameKey_Games_GameId",
                table: "GameKey",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
