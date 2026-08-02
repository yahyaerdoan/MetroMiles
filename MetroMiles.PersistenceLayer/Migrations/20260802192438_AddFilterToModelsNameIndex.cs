using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFilterToModelsNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Models_Name",
                table: "Models");

            migrationBuilder.CreateIndex(
                name: "UK_Models_Name",
                table: "Models",
                column: "Name",
                unique: true,
                filter: "[DeletedDate] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Models_Name",
                table: "Models");

            migrationBuilder.CreateIndex(
                name: "UK_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);
        }
    }
}
