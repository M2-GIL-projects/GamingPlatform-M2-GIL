using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabyrinthe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Labyrinth",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LobbyId",
                table: "Labyrinth",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "player1",
                table: "Labyrinth",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "player2",
                table: "Labyrinth",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "statep1",
                table: "Labyrinth",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "statep2",
                table: "Labyrinth",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Labyrinth_LobbyId",
                table: "Labyrinth",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labyrinth_Lobby_LobbyId",
                table: "Labyrinth",
                column: "LobbyId",
                principalTable: "Lobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labyrinth_Lobby_LobbyId",
                table: "Labyrinth");

            migrationBuilder.DropIndex(
                name: "IX_Labyrinth_LobbyId",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "player1",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "player2",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "statep1",
                table: "Labyrinth");

            migrationBuilder.DropColumn(
                name: "statep2",
                table: "Labyrinth");
        }
    }
}
