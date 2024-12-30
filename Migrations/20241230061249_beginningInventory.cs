using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class beginningInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeginningInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeterinaryValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialtyFeedsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgriculturalFeedsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DayOldChicksValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChemicalsOthersValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FertilizersSeedsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BeginningInventoryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeginningInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeginningInventories_BranchProducts_BranchId_ProductId",
                        columns: x => new { x.BranchId, x.ProductId },
                        principalTable: "BranchProducts",
                        principalColumns: new[] { "BranchId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventories_BranchId_ProductId",
                table: "BeginningInventories",
                columns: new[] { "BranchId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventoryDate",
                table: "BeginningInventories",
                column: "BeginningInventoryDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeginningInventories");
        }
    }
}
