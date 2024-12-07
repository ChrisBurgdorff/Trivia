using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalRDemo.Migrations
{
    public partial class QuestionNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentQuestion",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentQuestion",
                table: "Players");
        }
    }
}
