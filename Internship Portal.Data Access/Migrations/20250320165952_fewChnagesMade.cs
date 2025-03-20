using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class fewChnagesMade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "MentorAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorAllocations_StudentId1",
                table: "MentorAllocations",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorAllocations_StudentsData_StudentId1",
                table: "MentorAllocations",
                column: "StudentId1",
                principalTable: "StudentsData",
                principalColumn: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorAllocations_StudentsData_StudentId1",
                table: "MentorAllocations");

            migrationBuilder.DropIndex(
                name: "IX_MentorAllocations_StudentId1",
                table: "MentorAllocations");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "MentorAllocations");
        }
    }
}
