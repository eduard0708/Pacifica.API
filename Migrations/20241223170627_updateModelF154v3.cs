using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateModelF154v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotaSalesBreakDown",
                table: "F154SalesReports",
                newName: "TotalSalesBreakDown");

            migrationBuilder.RenameColumn(
                name: "CustomerCount",
                table: "F154SalesReports",
                newName: "CustomerCounts");

            migrationBuilder.RenameColumn(
                name: "AddTotalChecks",
                table: "F154SalesReports",
                newName: "TotalChecksAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSalesBreakDown",
                table: "F154SalesReports",
                newName: "TotaSalesBreakDown");

            migrationBuilder.RenameColumn(
                name: "TotalChecksAmount",
                table: "F154SalesReports",
                newName: "AddTotalChecks");

            migrationBuilder.RenameColumn(
                name: "CustomerCounts",
                table: "F154SalesReports",
                newName: "CustomerCount");
        }
    }
}
