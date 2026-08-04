using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddModelNormalizedNameAndRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Models",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Models",
                type: "nvarchar(450)",
                nullable: true,
                computedColumnSql: "UPPER([Name])",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_NormalizedName",
                table: "Models",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Models_NormalizedName",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Models");
        }
    }
}
