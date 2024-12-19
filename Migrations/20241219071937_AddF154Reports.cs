using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddF154Reports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailySalesReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SalesForTheDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCRM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalesCashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessOverPunch = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessSalesReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessChargeSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAccountability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    CashSlip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentsOfAccounts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherReceipts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CertifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CashDenominations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denomination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailySalesReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDenominations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashDenominations_DailySalesReports_DailySalesReportId",
                        column: x => x.DailySalesReportId,
                        principalTable: "DailySalesReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailySalesReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checks_DailySalesReports_DailySalesReportId",
                        column: x => x.DailySalesReportId,
                        principalTable: "DailySalesReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesBreakdowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategory = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailySalesReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesBreakdowns_DailySalesReports_DailySalesReportId",
                        column: x => x.DailySalesReportId,
                        principalTable: "DailySalesReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashDenominations_DailySalesReportId",
                table: "CashDenominations",
                column: "DailySalesReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_DailySalesReportId",
                table: "Checks",
                column: "DailySalesReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySalesReports_BranchId",
                table: "DailySalesReports",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesBreakdowns_DailySalesReportId",
                table: "SalesBreakdowns",
                column: "DailySalesReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashDenominations");

            migrationBuilder.DropTable(
                name: "Checks");

            migrationBuilder.DropTable(
                name: "SalesBreakdowns");

            migrationBuilder.DropTable(
                name: "DailySalesReports");
        }
    }
}
