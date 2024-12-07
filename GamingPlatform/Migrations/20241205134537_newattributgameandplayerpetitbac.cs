using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class newattributgameandplayerpetitbac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "PetitBacPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                table: "PetitBacPlayer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Pseudo",
                table: "PetitBacPlayer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "PetitBacPlayer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorPseudo",
                table: "PetitBacGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlayerCount",
                table: "PetitBacGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "PetitBacPlayer");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "PetitBacPlayer");

            migrationBuilder.DropColumn(
                name: "Pseudo",
                table: "PetitBacPlayer");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "PetitBacPlayer");

            migrationBuilder.DropColumn(
                name: "CreatorPseudo",
                table: "PetitBacGames");

            migrationBuilder.DropColumn(
                name: "PlayerCount",
                table: "PetitBacGames");
        }
    }
}
