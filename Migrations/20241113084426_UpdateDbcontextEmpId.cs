using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbcontextEmpId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeBranches_AspNetUsers_EmployeeId",
                table: "EmployeeBranches");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeBranches",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeBranches_AspNetUsers_Id",
                table: "EmployeeBranches",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeBranches_AspNetUsers_Id",
                table: "EmployeeBranches");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EmployeeBranches",
                newName: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeBranches_AspNetUsers_EmployeeId",
                table: "EmployeeBranches",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
