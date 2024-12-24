using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacifica.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_EmployeeId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_EmployeeId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "EmpoyeeRoles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpoyeeRoles",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_EmployeeId",
                table: "AspNetRoles",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_EmployeeId",
                table: "AspNetRoles",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
