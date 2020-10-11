using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lexica.EF.Migrations
{
    public partial class _01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "words");

            migrationBuilder.CreateTable(
                name: "Set",
                schema: "words",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Namespace = table.Column<string>(maxLength: 400, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entry",
                schema: "words",
                columns: table => new
                {
                    RecId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SetId = table.Column<long>(nullable: false),
                    EntryId = table.Column<int>(nullable: false),
                    Word = table.Column<string>(maxLength: 50, nullable: false),
                    Translation = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.RecId);
                    table.ForeignKey(
                        name: "FK_Entry_Set_SetId",
                        column: x => x.SetId,
                        principalSchema: "words",
                        principalTable: "Set",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImportHistory",
                schema: "words",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SetId = table.Column<long>(nullable: false),
                    ExecutedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportHistory_Set_SetId",
                        column: x => x.SetId,
                        principalSchema: "words",
                        principalTable: "Set",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_SetId",
                schema: "words",
                table: "Entry",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportHistory_SetId",
                schema: "words",
                table: "ImportHistory",
                column: "SetId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entry",
                schema: "words");

            migrationBuilder.DropTable(
                name: "ImportHistory",
                schema: "words");

            migrationBuilder.DropTable(
                name: "Set",
                schema: "words");
        }
    }
}
