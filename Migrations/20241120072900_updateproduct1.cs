using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateproduct1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "MinStockLevel",
                table: "BranchProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "BranchProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails");

            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "BranchProducts");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "BranchProducts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MinStockLevel",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAuditTrails_Products_ProductId",
                table: "ProductAuditTrails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
