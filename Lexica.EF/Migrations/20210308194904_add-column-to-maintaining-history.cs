using Microsoft.EntityFrameworkCore.Migrations;

namespace Lexica.EF.Migrations
{
    public partial class addcolumntomaintaininghistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_correct",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_correct",
                schema: "modes",
                table: "maintaining_history");
        }
    }
}
