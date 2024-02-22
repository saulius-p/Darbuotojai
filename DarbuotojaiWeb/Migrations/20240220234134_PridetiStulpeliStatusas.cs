using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarbuotojaiWeb.Migrations
{
    /// <inheritdoc />
    public partial class PridetiStulpeliStatusas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Statusas",
                table: "Darbuotojai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statusas",
                table: "Darbuotojai");
        }
    }
}
