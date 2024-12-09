using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateStockOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOuts_PaymentMethods_PaymentMethodId",
                table: "StockOuts");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "StockOuts");

            migrationBuilder.RenameColumn(
                name: "DateReported",
                table: "StockOuts",
                newName: "DateSold");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "StockOuts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "StockIns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_StockOuts_PaymentMethods_PaymentMethodId",
                table: "StockOuts",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOuts_PaymentMethods_PaymentMethodId",
                table: "StockOuts");

            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "StockIns");

            migrationBuilder.RenameColumn(
                name: "DateSold",
                table: "StockOuts",
                newName: "DateReported");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "StockOuts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "StockOuts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOuts_PaymentMethods_PaymentMethodId",
                table: "StockOuts",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id");
        }
    }
}
