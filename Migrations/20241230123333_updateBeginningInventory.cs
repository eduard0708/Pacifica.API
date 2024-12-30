using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateBeginningInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeginningInventories_BranchProducts_BranchId_ProductId",
                table: "BeginningInventories");

            migrationBuilder.DropIndex(
                name: "IX_BeginningInventories_BranchId_ProductId",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BeginningInventories");

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventories_BranchId",
                table: "BeginningInventories",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories");

            migrationBuilder.DropIndex(
                name: "IX_BeginningInventories_BranchId",
                table: "BeginningInventories");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BeginningInventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventories_BranchId_ProductId",
                table: "BeginningInventories",
                columns: new[] { "BranchId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BeginningInventories_BranchProducts_BranchId_ProductId",
                table: "BeginningInventories",
                columns: new[] { "BranchId", "ProductId" },
                principalTable: "BranchProducts",
                principalColumns: new[] { "BranchId", "ProductId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
