using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class InitAddProductStatusSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(762));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(765));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(766));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(1409));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(1856));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(1857));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(1857));

            migrationBuilder.InsertData(
                table: "ProductStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "IsActive", "ProductStatusName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(3381), null, null, "", true, "Available", null, null },
                    { 2, new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(3810), null, null, "", true, "OutOfStock", null, null }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(5353));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(6666));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(6670));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(6673));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(2380));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(2810));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(2813));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(2814));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(2814));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 91, DateTimeKind.Local).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 93, DateTimeKind.Local).AddTicks(4581));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 93, DateTimeKind.Local).AddTicks(4592));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 93, DateTimeKind.Local).AddTicks(4593));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(4373));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 55, 24, 94, DateTimeKind.Local).AddTicks(4794));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(5263));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(5266));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(5267));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(5268));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(5885));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(6314));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(6316));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(6317));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(6318));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(8917));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 341, DateTimeKind.Local).AddTicks(376));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 341, DateTimeKind.Local).AddTicks(380));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 341, DateTimeKind.Local).AddTicks(381));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 341, DateTimeKind.Local).AddTicks(383));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(7244));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(7245));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(7246));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 338, DateTimeKind.Local).AddTicks(3107));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 339, DateTimeKind.Local).AddTicks(9373));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 339, DateTimeKind.Local).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 339, DateTimeKind.Local).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(7872));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 52, 25, 340, DateTimeKind.Local).AddTicks(8354));
        }
    }
}
