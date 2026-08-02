using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameToBrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Brands",
                type: "nvarchar(450)",
                nullable: true,
                computedColumnSql: "UPPER([Name])",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_NormalizedName",
                table: "Brands",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Brands_NormalizedName",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Brands");
        }
    }
}
