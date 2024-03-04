using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarbuotojaiWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedPacientaiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Vardas",
                table: "Darbuotojai",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Pavarde",
                table: "Darbuotojai",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Adresas",
                table: "Darbuotojai",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Pacientai",
                columns: table => new
                {
                    PacientasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vardas = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Pavarde = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AsmensKodas = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Adresas = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Statusas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DarbuotojasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientai", x => x.PacientasId);
                    table.ForeignKey(
                        name: "FK_Pacientai_Darbuotojai_DarbuotojasId",
                        column: x => x.DarbuotojasId,
                        principalTable: "Darbuotojai",
                        principalColumn: "DarbuotojasId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientai_DarbuotojasId",
                table: "Pacientai",
                column: "DarbuotojasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientai");

            migrationBuilder.AlterColumn<string>(
                name: "Vardas",
                table: "Darbuotojai",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Pavarde",
                table: "Darbuotojai",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Adresas",
                table: "Darbuotojai",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
