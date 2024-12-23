using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashSlip",
                table: "F154SalesReports");

            migrationBuilder.DropColumn(
                name: "ChargeInvoice",
                table: "F154SalesReports");

            migrationBuilder.DropColumn(
                name: "OtherReceipts",
                table: "F154SalesReports");

            migrationBuilder.DropColumn(
                name: "PaymentsOfAccounts",
                table: "F154SalesReports");

            migrationBuilder.CreateTable(
                name: "InclusiveInvoiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InclusiveInvoiceTypes = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    F154SalesReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InclusiveInvoiceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InclusiveInvoiceTypes_F154SalesReports_F154SalesReportId",
                        column: x => x.F154SalesReportId,
                        principalTable: "F154SalesReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InclusiveInvoiceTypes_F154SalesReportId",
                table: "InclusiveInvoiceTypes",
                column: "F154SalesReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InclusiveInvoiceTypes");

            migrationBuilder.AddColumn<decimal>(
                name: "CashSlip",
                table: "F154SalesReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ChargeInvoice",
                table: "F154SalesReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherReceipts",
                table: "F154SalesReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentsOfAccounts",
                table: "F154SalesReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
