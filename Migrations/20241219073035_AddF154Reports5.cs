using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddF154Reports5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DailySalesReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DailySalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DailySalesReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DailySalesReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DailySalesReports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "DailySalesReports",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DailySalesReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DailySalesReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DailySalesReports");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DailySalesReports");
        }
    }
}
