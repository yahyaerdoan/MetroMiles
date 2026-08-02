using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToCarsPlate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UK_Cars_Plate",
                table: "Cars",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Cars_Plate",
                table: "Cars");
        }
    }
}
