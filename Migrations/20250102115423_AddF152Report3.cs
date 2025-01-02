using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddF152Report3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "F152ReportTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "F152ReportTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "F152ReportTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "F152ReportTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "F152ReportTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "F152ReportTransactions",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "F152ReportTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "F152ReportTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "F152ReportCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "F152ReportCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "F152ReportCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "F152ReportCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "F152ReportCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "F152ReportCategories",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "F152ReportCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "F152ReportCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "F152ReportTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "F152ReportCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "F152ReportCategories");
        }
    }
}
