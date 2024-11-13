using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProfiles_Adresses_AddressId",
                table: "EmployeeProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_Branches_BranchId",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_Branches_BranchId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_Products_ProductId",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_Products_ProductId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_TransactionReferences_TransactionReferenceId",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_TransactionReferences_TransactionReferenceId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_TransactionTypes_TransactionTypeId",
                table: "stockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_stockTransactionInOuts_TransactionTypes_TransactionTypeId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stockTransactionInOuts",
                table: "stockTransactionInOuts");

            migrationBuilder.DropIndex(
                name: "IX_stockTransactionInOuts_BranchId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropIndex(
                name: "IX_stockTransactionInOuts_ProductId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropIndex(
                name: "IX_stockTransactionInOuts_TransactionReferenceId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropIndex(
                name: "IX_stockTransactionInOuts_TransactionTypeId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adresses",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropColumn(
                name: "TransactionReferenceId1",
                table: "stockTransactionInOuts");

            migrationBuilder.DropColumn(
                name: "TransactionTypeId1",
                table: "stockTransactionInOuts");

            migrationBuilder.RenameTable(
                name: "stockTransactionInOuts",
                newName: "StockTransactionInOuts");

            migrationBuilder.RenameTable(
                name: "Adresses",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_stockTransactionInOuts_TransactionTypeId",
                table: "StockTransactionInOuts",
                newName: "IX_StockTransactionInOuts_TransactionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_stockTransactionInOuts_TransactionReferenceId",
                table: "StockTransactionInOuts",
                newName: "IX_StockTransactionInOuts_TransactionReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_stockTransactionInOuts_ProductId",
                table: "StockTransactionInOuts",
                newName: "IX_StockTransactionInOuts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_stockTransactionInOuts_BranchId",
                table: "StockTransactionInOuts",
                newName: "IX_StockTransactionInOuts_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTransactionInOuts",
                table: "StockTransactionInOuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProfiles_Addresses_AddressId",
                table: "EmployeeProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionInOuts_Branches_BranchId",
                table: "StockTransactionInOuts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionInOuts_Products_ProductId",
                table: "StockTransactionInOuts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionInOuts_TransactionReferences_TransactionReferenceId",
                table: "StockTransactionInOuts",
                column: "TransactionReferenceId",
                principalTable: "TransactionReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionInOuts_TransactionTypes_TransactionTypeId",
                table: "StockTransactionInOuts",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProfiles_Addresses_AddressId",
                table: "EmployeeProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionInOuts_Branches_BranchId",
                table: "StockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionInOuts_Products_ProductId",
                table: "StockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionInOuts_TransactionReferences_TransactionReferenceId",
                table: "StockTransactionInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionInOuts_TransactionTypes_TransactionTypeId",
                table: "StockTransactionInOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTransactionInOuts",
                table: "StockTransactionInOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "StockTransactionInOuts",
                newName: "stockTransactionInOuts");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Adresses");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionInOuts_TransactionTypeId",
                table: "stockTransactionInOuts",
                newName: "IX_stockTransactionInOuts_TransactionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionInOuts_TransactionReferenceId",
                table: "stockTransactionInOuts",
                newName: "IX_stockTransactionInOuts_TransactionReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionInOuts_ProductId",
                table: "stockTransactionInOuts",
                newName: "IX_stockTransactionInOuts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionInOuts_BranchId",
                table: "stockTransactionInOuts",
                newName: "IX_stockTransactionInOuts_BranchId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId1",
                table: "stockTransactionInOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "stockTransactionInOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionReferenceId1",
                table: "stockTransactionInOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeId1",
                table: "stockTransactionInOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_stockTransactionInOuts",
                table: "stockTransactionInOuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adresses",
                table: "Adresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_stockTransactionInOuts_BranchId1",
                table: "stockTransactionInOuts",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_stockTransactionInOuts_ProductId1",
                table: "stockTransactionInOuts",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_stockTransactionInOuts_TransactionReferenceId1",
                table: "stockTransactionInOuts",
                column: "TransactionReferenceId1");

            migrationBuilder.CreateIndex(
                name: "IX_stockTransactionInOuts_TransactionTypeId1",
                table: "stockTransactionInOuts",
                column: "TransactionTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProfiles_Adresses_AddressId",
                table: "EmployeeProfiles",
                column: "AddressId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_Branches_BranchId",
                table: "stockTransactionInOuts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_Branches_BranchId1",
                table: "stockTransactionInOuts",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_Products_ProductId",
                table: "stockTransactionInOuts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_Products_ProductId1",
                table: "stockTransactionInOuts",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_TransactionReferences_TransactionReferenceId",
                table: "stockTransactionInOuts",
                column: "TransactionReferenceId",
                principalTable: "TransactionReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_TransactionReferences_TransactionReferenceId1",
                table: "stockTransactionInOuts",
                column: "TransactionReferenceId1",
                principalTable: "TransactionReferences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_TransactionTypes_TransactionTypeId",
                table: "stockTransactionInOuts",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stockTransactionInOuts_TransactionTypes_TransactionTypeId1",
                table: "stockTransactionInOuts",
                column: "TransactionTypeId1",
                principalTable: "TransactionTypes",
                principalColumn: "Id");
        }
    }
}
