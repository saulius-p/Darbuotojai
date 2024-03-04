using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarbuotojaiWeb.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullDarbuotojasId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientai_Darbuotojai_DarbuotojasId",
                table: "Pacientai");

            migrationBuilder.AlterColumn<int>(
                name: "DarbuotojasId",
                table: "Pacientai",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientai_Darbuotojai_DarbuotojasId",
                table: "Pacientai",
                column: "DarbuotojasId",
                principalTable: "Darbuotojai",
                principalColumn: "DarbuotojasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientai_Darbuotojai_DarbuotojasId",
                table: "Pacientai");

            migrationBuilder.AlterColumn<int>(
                name: "DarbuotojasId",
                table: "Pacientai",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientai_Darbuotojai_DarbuotojasId",
                table: "Pacientai",
                column: "DarbuotojasId",
                principalTable: "Darbuotojai",
                principalColumn: "DarbuotojasId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
