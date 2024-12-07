using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetitBacPlayerResponses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFinished",
                table: "PetitBacPlayer");

            migrationBuilder.RenameColumn(
                name: "PlayerName",
                table: "PetitBacPlayer",
                newName: "Responses");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PetitBacPlayer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PetitBacPlayer");

            migrationBuilder.RenameColumn(
                name: "Responses",
                table: "PetitBacPlayer",
                newName: "PlayerName");

            migrationBuilder.AddColumn<bool>(
                name: "HasFinished",
                table: "PetitBacPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
