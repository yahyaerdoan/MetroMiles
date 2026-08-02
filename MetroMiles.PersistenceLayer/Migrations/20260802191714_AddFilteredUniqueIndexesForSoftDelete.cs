using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFilteredUniqueIndexesForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Transmissions_Name",
                table: "Transmissions");

            migrationBuilder.DropIndex(
                name: "UK_Fuels_Name",
                table: "Fuels");

            migrationBuilder.DropIndex(
                name: "UK_Cars_Plate",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "UK_Brands_Name",
                table: "Brands");

            migrationBuilder.CreateIndex(
                name: "UK_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UK_Transmissions_Name",
                table: "Transmissions",
                column: "Name",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UK_Fuels_Name",
                table: "Fuels",
                column: "Name",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UK_Cars_Plate",
                table: "Cars",
                column: "Plate",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UK_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true,
                filter: "[DeletedDate] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UK_Transmissions_Name",
                table: "Transmissions");

            migrationBuilder.DropIndex(
                name: "UK_Fuels_Name",
                table: "Fuels");

            migrationBuilder.DropIndex(
                name: "UK_Cars_Plate",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "UK_Brands_Name",
                table: "Brands");

            migrationBuilder.CreateIndex(
                name: "UK_Transmissions_Name",
                table: "Transmissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Fuels_Name",
                table: "Fuels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Cars_Plate",
                table: "Cars",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);
        }
    }
}
