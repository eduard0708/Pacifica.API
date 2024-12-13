using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscrepancyValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescripancyValue",
                table: "InventoryNormalizations",
                newName: "SystemQuantity");

            migrationBuilder.RenameColumn(
                name: "SumDiscrepancyValue",
                table: "Inventories",
                newName: "DiscrepancyValue");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualQuantity",
                table: "InventoryNormalizations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "InventoryNormalizations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscrepancyValue",
                table: "InventoryNormalizations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualQuantity",
                table: "InventoryNormalizations");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "InventoryNormalizations");

            migrationBuilder.DropColumn(
                name: "DiscrepancyValue",
                table: "InventoryNormalizations");

            migrationBuilder.RenameColumn(
                name: "SystemQuantity",
                table: "InventoryNormalizations",
                newName: "DescripancyValue");

            migrationBuilder.RenameColumn(
                name: "DiscrepancyValue",
                table: "Inventories",
                newName: "SumDiscrepancyValue");
        }
    }
}
