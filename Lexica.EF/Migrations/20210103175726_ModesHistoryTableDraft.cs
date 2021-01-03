using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lexica.EF.Migrations
{
    public partial class ModesHistoryTableDraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "modes");

            migrationBuilder.CreateTable(
                name: "MaintainingHistory",
                schema: "modes",
                columns: table => new
                {
                    OperationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRecId = table.Column<long>(nullable: false),
                    IsWord = table.Column<bool>(nullable: false),
                    IsTranslation = table.Column<bool>(nullable: false),
                    NumOfCorrectAnswers = table.Column<long>(nullable: false),
                    NumOfMistakes = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintainingHistory", x => x.OperationId);
                    table.ForeignKey(
                        name: "FK_MaintainingHistory_Entry_EntryRecId",
                        column: x => x.EntryRecId,
                        principalSchema: "words",
                        principalTable: "Entry",
                        principalColumn: "RecId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingHistory_EntryRecId",
                schema: "modes",
                table: "MaintainingHistory",
                column: "EntryRecId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintainingHistory",
                schema: "modes");
        }
    }
}
