using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class addextra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RollNumber",
                table: "StudentsData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "StudentsData");
        }
    }
}
