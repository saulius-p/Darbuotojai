using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DarbuotojaiWeb.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Darbuotojai",
                columns: table => new
                {
                    DarbuotojasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vardas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pavarde = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GimimoData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Darbuotojai", x => x.DarbuotojasId);
                });

            migrationBuilder.CreateTable(
                name: "Pareigos",
                columns: table => new
                {
                    PareigaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pavadinimas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pareigos", x => x.PareigaId);
                });

            migrationBuilder.CreateTable(
                name: "DarbuotojasPareigos",
                columns: table => new
                {
                    DarbuotojasId = table.Column<int>(type: "int", nullable: false),
                    PareigaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DarbuotojasPareigos", x => new { x.DarbuotojasId, x.PareigaId });
                    table.ForeignKey(
                        name: "FK_DarbuotojasPareigos_Darbuotojai_DarbuotojasId",
                        column: x => x.DarbuotojasId,
                        principalTable: "Darbuotojai",
                        principalColumn: "DarbuotojasId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DarbuotojasPareigos_Pareigos_PareigaId",
                        column: x => x.PareigaId,
                        principalTable: "Pareigos",
                        principalColumn: "PareigaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Pareigos",
                columns: new[] { "PareigaId", "Pavadinimas" },
                values: new object[,]
                {
                    { 1, "Administratorius(-ė)" },
                    { 2, "Analitikas(-ė)" },
                    { 3, "Bendrosios praktikos slaugytoja" },
                    { 4, "Laborantas(-ė)" },
                    { 5, "Vadovas(-ė)" },
                    { 6, "Apskaitininkas(-ė)" },
                    { 7, "Programuotojas(-a)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DarbuotojasPareigos_PareigaId",
                table: "DarbuotojasPareigos",
                column: "PareigaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DarbuotojasPareigos");

            migrationBuilder.DropTable(
                name: "Darbuotojai");

            migrationBuilder.DropTable(
                name: "Pareigos");
        }
    }
}
