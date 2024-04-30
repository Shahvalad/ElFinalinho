using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedListingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Listings");
        }
    }
}
