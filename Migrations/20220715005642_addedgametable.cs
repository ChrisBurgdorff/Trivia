using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalRDemo.Migrations
{
    public partial class addedgametable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameModelId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameModelId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_GameModelId",
                table: "Questions",
                column: "GameModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameModelId",
                table: "Players",
                column: "GameModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_GameModelId",
                table: "Players",
                column: "GameModelId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Games_GameModelId",
                table: "Questions",
                column: "GameModelId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_GameModelId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Games_GameModelId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Questions_GameModelId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Players_GameModelId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameModelId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "GameModelId",
                table: "Players");
        }
    }
}
