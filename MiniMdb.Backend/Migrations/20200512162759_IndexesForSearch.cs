using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniMdb.Backend.Migrations
{
    public partial class IndexesForSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Titles_Name",
                table: "Titles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_Type",
                table: "Titles",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Titles_Name",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_Type",
                table: "Titles");
        }
    }
}
