using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetitBacGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    EndCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeLimit = table.Column<int>(type: "int", nullable: false),
                    IsGameStarted = table.Column<bool>(type: "bit", nullable: false),
                    LobbyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitBacGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetitBacGames_Lobby_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetitBacPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasFinished = table.Column<bool>(type: "bit", nullable: false),
                    PetitBacGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitBacPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetitBacPlayer_PetitBacGames_PetitBacGameId",
                        column: x => x.PetitBacGameId,
                        principalTable: "PetitBacGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetitBacPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_PetitBacPlayer_PetitBacPlayerId",
                        column: x => x.PetitBacPlayerId,
                        principalTable: "PetitBacPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_PetitBacPlayerId",
                table: "Answers",
                column: "PetitBacPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PetitBacGames_LobbyId",
                table: "PetitBacGames",
                column: "LobbyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetitBacPlayer_PetitBacGameId",
                table: "PetitBacPlayer",
                column: "PetitBacGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "PetitBacPlayer");

            migrationBuilder.DropTable(
                name: "PetitBacGames");
        }
    }
}
