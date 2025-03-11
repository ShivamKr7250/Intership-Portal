using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class modifiedBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogThumnail",
                table: "Blogs",
                newName: "BlogThumbnail");

            migrationBuilder.AlterColumn<string>(
                name: "WorkAuthorization",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PreferredJobLocation",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PlacedCompany",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LanguagesKnown",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "InternshipExperience",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ExtracurricularActivities",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Certifications",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNotes",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDeadline",
                table: "Blogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Batch",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Blogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasYearGap",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaximumPostGraduationCGPA",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumCGPA",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumGraduationPercentage",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumInterPercentage",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumMatricPercentage",
                table: "Blogs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationDeadline",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Batch",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "HasYearGap",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MaximumPostGraduationCGPA",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MinimumCGPA",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MinimumGraduationPercentage",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MinimumInterPercentage",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MinimumMatricPercentage",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "BlogThumbnail",
                table: "Blogs",
                newName: "BlogThumnail");

            migrationBuilder.AlterColumn<string>(
                name: "WorkAuthorization",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreferredJobLocation",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlacedCompany",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguagesKnown",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternshipExperience",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtracurricularActivities",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Certifications",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNotes",
                table: "StudentsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
