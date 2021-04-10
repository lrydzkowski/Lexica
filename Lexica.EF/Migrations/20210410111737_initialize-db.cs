using Microsoft.EntityFrameworkCore.Migrations;

namespace Lexica.EF.Migrations
{
    public partial class initializedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "modes");

            migrationBuilder.CreateTable(
                name: "learning_history",
                schema: "modes",
                columns: table => new
                {
                    operation_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    @namespace = table.Column<string>(name: "namespace", type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    mode = table.Column<string>(type: "TEXT", nullable: false),
                    question = table.Column<string>(type: "TEXT", nullable: false),
                    question_type = table.Column<string>(type: "TEXT", nullable: false),
                    answer = table.Column<string>(type: "TEXT", nullable: false),
                    proper_answer = table.Column<string>(type: "TEXT", nullable: false),
                    is_correct = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_history", x => x.operation_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "learning_history",
                schema: "modes");
        }
    }
}
