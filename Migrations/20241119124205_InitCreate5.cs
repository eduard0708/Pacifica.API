using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAuditTrailProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductAuditTrails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAuditTrails_ProductId",
                table: "ProductAuditTrails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_ProductAuditTrails_ProductId",
                table: "ProductAuditTrails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductAuditTrails");

            migrationBuilder.CreateTable(
                name: "ProductAuditTrailProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductAuditTrailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAuditTrailProducts", x => new { x.ProductId, x.ProductAuditTrailId });
                    table.ForeignKey(
                        name: "FK_ProductAuditTrailProducts_ProductAuditTrails_ProductAuditTrailId",
                        column: x => x.ProductAuditTrailId,
                        principalTable: "ProductAuditTrails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAuditTrailProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAuditTrailProducts_ProductAuditTrailId",
                table: "ProductAuditTrailProducts",
                column: "ProductAuditTrailId");
        }
    }
}
