using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchProductAuditTrails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "ProductAuditTrails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BranchProductAuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    EntityBranchId = table.Column<int>(type: "int", nullable: true),
                    EntityProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchProductAuditTrails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchProductAuditTrails_BranchProducts_BranchId_ProductId",
                        columns: x => new { x.BranchId, x.ProductId },
                        principalTable: "BranchProducts",
                        principalColumns: new[] { "BranchId", "ProductId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchProductAuditTrails_BranchProducts_EntityBranchId_EntityProductId",
                        columns: x => new { x.EntityBranchId, x.EntityProductId },
                        principalTable: "BranchProducts",
                        principalColumns: new[] { "BranchId", "ProductId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAuditTrails_EntityId",
                table: "ProductAuditTrails",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProductAuditTrails_BranchId_ProductId",
                table: "BranchProductAuditTrails",
                columns: new[] { "BranchId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_BranchProductAuditTrails_EntityBranchId_EntityProductId",
                table: "BranchProductAuditTrails",
                columns: new[] { "EntityBranchId", "EntityProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAuditTrails_Products_EntityId",
                table: "ProductAuditTrails",
                column: "EntityId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAuditTrails_Products_EntityId",
                table: "ProductAuditTrails");

            migrationBuilder.DropTable(
                name: "BranchProductAuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_ProductAuditTrails_EntityId",
                table: "ProductAuditTrails");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "ProductAuditTrails");
        }
    }
}
