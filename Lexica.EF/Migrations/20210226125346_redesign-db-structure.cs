using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lexica.EF.Migrations
{
    public partial class redesigndbstructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_maintaining_history_entry_entry_rec_id",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropTable(
                name: "entry",
                schema: "words");

            migrationBuilder.DropTable(
                name: "import_history",
                schema: "words");

            migrationBuilder.DropTable(
                name: "set",
                schema: "words");

            migrationBuilder.DropIndex(
                name: "IX_maintaining_history_entry_rec_id",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "entry_rec_id",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "is_translation",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "is_word",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "num_of_correct_answers",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "num_of_mistakes",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.AddColumn<string>(
                name: "answer",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "namespace",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "proper_answer",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "question",
                schema: "modes",
                table: "maintaining_history",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "answer",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "name",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "namespace",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "proper_answer",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.DropColumn(
                name: "question",
                schema: "modes",
                table: "maintaining_history");

            migrationBuilder.EnsureSchema(
                name: "words");

            migrationBuilder.AddColumn<long>(
                name: "entry_rec_id",
                schema: "modes",
                table: "maintaining_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "is_translation",
                schema: "modes",
                table: "maintaining_history",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_word",
                schema: "modes",
                table: "maintaining_history",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "num_of_correct_answers",
                schema: "modes",
                table: "maintaining_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "num_of_mistakes",
                schema: "modes",
                table: "maintaining_history",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "set",
                schema: "words",
                columns: table => new
                {
                    set_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    @namespace = table.Column<string>(name: "namespace", type: "character varying(400)", maxLength: 400, nullable: false)
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
                    rec_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_id = table.Column<int>(type: "integer", nullable: false),
                    set_id = table.Column<long>(type: "bigint", nullable: false),
                    translation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    word = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    import_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    executed_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    set_id = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_maintaining_history_entry_entry_rec_id",
                schema: "modes",
                table: "maintaining_history",
                column: "entry_rec_id",
                principalSchema: "words",
                principalTable: "entry",
                principalColumn: "rec_id");
        }
    }
}
