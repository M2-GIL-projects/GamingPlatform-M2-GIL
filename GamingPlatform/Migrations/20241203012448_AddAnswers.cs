using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_PetitBacPlayer_PetitBacPlayerId",
                table: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "SelectedCategories",
                table: "PetitBacGames");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_PetitBacPlayerId",
                table: "Answer",
                newName: "IX_Answer_PetitBacPlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PetitBacCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetitBacGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitBacCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetitBacCategories_PetitBacGames_PetitBacGameId",
                        column: x => x.PetitBacGameId,
                        principalTable: "PetitBacGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetitBacCategories_PetitBacGameId",
                table: "PetitBacCategories",
                column: "PetitBacGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_PetitBacPlayer_PetitBacPlayerId",
                table: "Answer",
                column: "PetitBacPlayerId",
                principalTable: "PetitBacPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_PetitBacPlayer_PetitBacPlayerId",
                table: "Answer");

            migrationBuilder.DropTable(
                name: "PetitBacCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_PetitBacPlayerId",
                table: "Answers",
                newName: "IX_Answers_PetitBacPlayerId");

            migrationBuilder.AddColumn<string>(
                name: "SelectedCategories",
                table: "PetitBacGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_PetitBacPlayer_PetitBacPlayerId",
                table: "Answers",
                column: "PetitBacPlayerId",
                principalTable: "PetitBacPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
