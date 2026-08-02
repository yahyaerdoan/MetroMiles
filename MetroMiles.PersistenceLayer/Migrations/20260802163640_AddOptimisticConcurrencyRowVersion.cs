using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetroMiles.PersistenceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddOptimisticConcurrencyRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Transmissions",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Fuels",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Cars",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Brands",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Transmissions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Fuels");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Brands");
        }
    }
}
