using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class AddF152Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "BeginningInventories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "F152ReportTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_F152ReportTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_F152ReportTransactions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "F152ReportCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostWeek1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailWeek1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostWeek2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailWeek2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostWeek3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailWeek3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostWeek4 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailWeek4 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentRetail = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    F152ReportTransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_F152ReportCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_F152ReportCategories_F152ReportTransactions_F152ReportTransactionId",
                        column: x => x.F152ReportTransactionId,
                        principalTable: "F152ReportTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_F152ReportCategories_F152ReportTransactionId",
                table: "F152ReportCategories",
                column: "F152ReportTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_F152ReportTransactions_BranchId",
                table: "F152ReportTransactions",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "F152ReportCategories");

            migrationBuilder.DropTable(
                name: "F152ReportTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "BeginningInventories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
