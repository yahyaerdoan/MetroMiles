using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedPlateToCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Cars",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedPlate",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: true,
                computedColumnSql: "UPPER([Plate])",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_NormalizedPlate",
                table: "Cars",
                column: "NormalizedPlate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_NormalizedPlate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "NormalizedPlate",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
