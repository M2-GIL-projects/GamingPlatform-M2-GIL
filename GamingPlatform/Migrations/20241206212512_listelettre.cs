using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class listelettre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Letter",
                table: "PetitBacGames");

            migrationBuilder.AddColumn<string>(
                name: "Letters",
                table: "PetitBacGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Letters",
                table: "PetitBacGames");

            migrationBuilder.AddColumn<string>(
                name: "Letter",
                table: "PetitBacGames",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }
    }
}
