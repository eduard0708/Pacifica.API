using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class InitCreateSeedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(7935));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(8454));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(8456));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(8457));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(8458));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(9579));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "DateAdded", "DeletedAt", "IsActive", "LastUpdated", "MinStockLevel", "ProductName", "ProductStatus", "ReorderLevel", "SKU", "SupplierId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(2183), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Fish Food A", "Available", 0, "SKU001", 1, null, null },
                    { 2, 2, new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(3476), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Aquarium Filter", "OutOfStock", 0, "SKU002", 2, null, null },
                    { 3, 3, new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(3480), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Hog Feed B", "Available", 0, "SKU003", 3, null, null },
                    { 4, 4, new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(3481), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Chicken Feed C", "Available", 0, "SKU004", 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "DateAdded", "DeletedAt", "LastUpdated", "MinStockLevel", "ProductName", "ProductStatus", "ReorderLevel", "SKU", "SupplierId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 5, 5, new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(3482), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Bird Feed D", "OutOfStock", 0, "SKU005", 5, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(109));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 827, DateTimeKind.Local).AddTicks(6682));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 829, DateTimeKind.Local).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 17, 49, 830, DateTimeKind.Local).AddTicks(1630));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(6117));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(6620));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(7671));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(7674));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(7675));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(7676));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(8184));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 289, DateTimeKind.Local).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(462));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(472));

            migrationBuilder.UpdateData(
                table: "TransactionReferences",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(9248));

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 14, 8, 3, 36, 291, DateTimeKind.Local).AddTicks(9656));
        }
    }
}
