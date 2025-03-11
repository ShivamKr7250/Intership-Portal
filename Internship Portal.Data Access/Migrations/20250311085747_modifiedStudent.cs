using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class modifiedStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudentsData",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsData_UserId",
                table: "StudentsData",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsData_AspNetUsers_UserId",
                table: "StudentsData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsData_AspNetUsers_UserId",
                table: "StudentsData");

            migrationBuilder.DropIndex(
                name: "IX_StudentsData_UserId",
                table: "StudentsData");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentsData");
        }
    }
}
