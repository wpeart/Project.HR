using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.HR.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateComplete4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_UserRoles_UserRolesRoleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserRolesRoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserRolesRoleId",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRolesRoleId",
                table: "Employees",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserRolesRoleId",
                table: "Employees",
                column: "UserRolesRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_UserRoles_UserRolesRoleId",
                table: "Employees",
                column: "UserRolesRoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleId");
        }
    }
}
