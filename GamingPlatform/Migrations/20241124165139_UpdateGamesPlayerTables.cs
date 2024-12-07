using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGamesPlayerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Game_GameCode",
                table: "Lobby");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobby_Game_GameCode",
                table: "Lobby",
                column: "GameCode",
                principalTable: "Game",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Game_GameCode",
                table: "Lobby");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Player",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lobby_Game_GameCode",
                table: "Lobby",
                column: "GameCode",
                principalTable: "Game",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
