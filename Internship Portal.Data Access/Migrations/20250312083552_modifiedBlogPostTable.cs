using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class modifiedBlogPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Blogs");
        }
    }
}
