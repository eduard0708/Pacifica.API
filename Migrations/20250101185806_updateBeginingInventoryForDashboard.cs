using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class updateBeginingInventoryForDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories");

            migrationBuilder.DropIndex(
                name: "IX_BeginningInventoryDate",
                table: "BeginningInventories");

            migrationBuilder.AddColumn<string>(
                name: "AgriculturalFeedsColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AgriculturalFeedsIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AgriculturalFeedsName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChemicalsOthersColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChemicalsOthersIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChemicalsOthersName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DayOldChicksColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DayOldChicksIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DayOldChicksName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FertilizersSeedsColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FertilizersSeedsIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FertilizersSeedsName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyFeedsColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyFeedsIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyFeedsName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VeterinaryColor",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VeterinaryIcon",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VeterinaryName",
                table: "BeginningInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventoryDate",
                table: "BeginningInventories",
                column: "BeginningInventoryDate",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories");

            migrationBuilder.DropIndex(
                name: "IX_BeginningInventoryDate",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "AgriculturalFeedsColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "AgriculturalFeedsIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "AgriculturalFeedsName",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "ChemicalsOthersColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "ChemicalsOthersIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "ChemicalsOthersName",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "DayOldChicksColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "DayOldChicksIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "DayOldChicksName",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "FertilizersSeedsColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "FertilizersSeedsIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "FertilizersSeedsName",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "SpecialtyFeedsColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "SpecialtyFeedsIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "SpecialtyFeedsName",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "VeterinaryColor",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "VeterinaryIcon",
                table: "BeginningInventories");

            migrationBuilder.DropColumn(
                name: "VeterinaryName",
                table: "BeginningInventories");

            migrationBuilder.CreateIndex(
                name: "IX_BeginningInventoryDate",
                table: "BeginningInventories",
                column: "BeginningInventoryDate");

            migrationBuilder.AddForeignKey(
                name: "FK_BeginningInventories_Branches_BranchId",
                table: "BeginningInventories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
