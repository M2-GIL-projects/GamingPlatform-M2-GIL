using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndCondition",
                table: "PetitBacGames");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "PetitBacGames");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PetitBacPlayer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PetitBacPlayer");

            migrationBuilder.AddColumn<string>(
                name: "EndCondition",
                table: "PetitBacGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "PetitBacGames",
                type: "int",
                nullable: true);
        }
    }
}
