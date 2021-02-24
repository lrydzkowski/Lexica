using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lexica.EF.Migrations
{
    public partial class dbinitialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "words");

            migrationBuilder.EnsureSchema(
                name: "modes");

            migrationBuilder.CreateTable(
                name: "set",
                schema: "words",
                columns: table => new
                {
                    set_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    @namespace = table.Column<string>(name: "namespace", maxLength: 400, nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set", x => x.set_id);
                });

            migrationBuilder.CreateTable(
                name: "entry",
                schema: "words",
                columns: table => new
                {
                    rec_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    set_id = table.Column<long>(nullable: false),
                    entry_id = table.Column<int>(nullable: false),
                    word = table.Column<string>(maxLength: 50, nullable: false),
                    translation = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry", x => x.rec_id);
                    table.ForeignKey(
                        name: "FK_entry_set_set_id",
                        column: x => x.set_id,
                        principalSchema: "words",
                        principalTable: "set",
                        principalColumn: "set_id");
                });

            migrationBuilder.CreateTable(
                name: "import_history",
                schema: "words",
                columns: table => new
                {
                    import_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    set_id = table.Column<long>(nullable: false),
                    executed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_history", x => x.import_id);
                    table.ForeignKey(
                        name: "FK_import_history_set_set_id",
                        column: x => x.set_id,
                        principalSchema: "words",
                        principalTable: "set",
                        principalColumn: "set_id");
                });

            migrationBuilder.CreateTable(
                name: "maintaining_history",
                schema: "modes",
                columns: table => new
                {
                    operation_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_rec_id = table.Column<long>(nullable: false),
                    is_word = table.Column<bool>(nullable: false),
                    is_translation = table.Column<bool>(nullable: false),
                    num_of_correct_answers = table.Column<long>(nullable: false),
                    num_of_mistakes = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintaining_history", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_maintaining_history_entry_entry_rec_id",
                        column: x => x.entry_rec_id,
                        principalSchema: "words",
                        principalTable: "entry",
                        principalColumn: "rec_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_maintaining_history_entry_rec_id",
                schema: "modes",
                table: "maintaining_history",
                column: "entry_rec_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entry_set_id",
                schema: "words",
                table: "entry",
                column: "set_id");

            migrationBuilder.CreateIndex(
                name: "IX_import_history_set_id",
                schema: "words",
                table: "import_history",
                column: "set_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintaining_history",
                schema: "modes");

            migrationBuilder.DropTable(
                name: "import_history",
                schema: "words");

            migrationBuilder.DropTable(
                name: "entry",
                schema: "words");

            migrationBuilder.DropTable(
                name: "set",
                schema: "words");
        }
    }
}
