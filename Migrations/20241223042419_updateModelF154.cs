using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateModelF154 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashDenominations_DailySalesReports_DailySalesReportId",
                table: "CashDenominations");

            migrationBuilder.DropForeignKey(
                name: "FK_Checks_DailySalesReports_DailySalesReportId",
                table: "Checks");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesBreakdowns_DailySalesReports_DailySalesReportId",
                table: "SalesBreakdowns");

            migrationBuilder.DropTable(
                name: "DailySalesReports");

            migrationBuilder.RenameColumn(
                name: "DailySalesReportId",
                table: "SalesBreakdowns",
                newName: "F154SalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesBreakdowns_DailySalesReportId",
                table: "SalesBreakdowns",
                newName: "IX_SalesBreakdowns_F154SalesReportId");

            migrationBuilder.RenameColumn(
                name: "DailySalesReportId",
                table: "Checks",
                newName: "F154SalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Checks_DailySalesReportId",
                table: "Checks",
                newName: "IX_Checks_F154SalesReportId");

            migrationBuilder.RenameColumn(
                name: "DailySalesReportId",
                table: "CashDenominations",
                newName: "F154SalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_CashDenominations_DailySalesReportId",
                table: "CashDenominations",
                newName: "IX_CashDenominations_F154SalesReportId");

            migrationBuilder.CreateTable(
                name: "F154SalesReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateReported = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SalesForTheDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCRM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OverAllTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAccountability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDenomination = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddTotalChecks = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCashCount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashShortOver = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerCapita = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotaSalesBreakDown = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    CashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentsOfAccounts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherReceipts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CertifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_F154SalesReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_F154SalesReports_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverPunch = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesReturnOP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    F154SalesReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesses_F154SalesReports_F154SalesReportId",
                        column: x => x.F154SalesReportId,
                        principalTable: "F154SalesReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_F154SalesReports_BranchId",
                table: "F154SalesReports",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesses_F154SalesReportId",
                table: "Lesses",
                column: "F154SalesReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashDenominations_F154SalesReports_F154SalesReportId",
                table: "CashDenominations",
                column: "F154SalesReportId",
                principalTable: "F154SalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checks_F154SalesReports_F154SalesReportId",
                table: "Checks",
                column: "F154SalesReportId",
                principalTable: "F154SalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesBreakdowns_F154SalesReports_F154SalesReportId",
                table: "SalesBreakdowns",
                column: "F154SalesReportId",
                principalTable: "F154SalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashDenominations_F154SalesReports_F154SalesReportId",
                table: "CashDenominations");

            migrationBuilder.DropForeignKey(
                name: "FK_Checks_F154SalesReports_F154SalesReportId",
                table: "Checks");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesBreakdowns_F154SalesReports_F154SalesReportId",
                table: "SalesBreakdowns");

            migrationBuilder.DropTable(
                name: "Lesses");

            migrationBuilder.DropTable(
                name: "F154SalesReports");

            migrationBuilder.RenameColumn(
                name: "F154SalesReportId",
                table: "SalesBreakdowns",
                newName: "DailySalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesBreakdowns_F154SalesReportId",
                table: "SalesBreakdowns",
                newName: "IX_SalesBreakdowns_DailySalesReportId");

            migrationBuilder.RenameColumn(
                name: "F154SalesReportId",
                table: "Checks",
                newName: "DailySalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Checks_F154SalesReportId",
                table: "Checks",
                newName: "IX_Checks_DailySalesReportId");

            migrationBuilder.RenameColumn(
                name: "F154SalesReportId",
                table: "CashDenominations",
                newName: "DailySalesReportId");

            migrationBuilder.RenameIndex(
                name: "IX_CashDenominations_F154SalesReportId",
                table: "CashDenominations",
                newName: "IX_CashDenominations_DailySalesReportId");

            migrationBuilder.CreateTable(
                name: "DailySalesReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CertifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DenominationSumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCRM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LessChargeSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessOverPunch = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessSalesReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAccountability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherReceipts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentsOfAccounts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    SalesForTheDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySalesReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySalesReports_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailySalesReports_BranchId",
                table: "DailySalesReports",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashDenominations_DailySalesReports_DailySalesReportId",
                table: "CashDenominations",
                column: "DailySalesReportId",
                principalTable: "DailySalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checks_DailySalesReports_DailySalesReportId",
                table: "Checks",
                column: "DailySalesReportId",
                principalTable: "DailySalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesBreakdowns_DailySalesReports_DailySalesReportId",
                table: "SalesBreakdowns",
                column: "DailySalesReportId",
                principalTable: "DailySalesReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
