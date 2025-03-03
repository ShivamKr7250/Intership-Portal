using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internship_Portal.Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class addStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentsData",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Batch = table.Column<int>(type: "int", nullable: false),
                    Backlogs = table.Column<int>(type: "int", nullable: false),
                    MatricPercentage = table.Column<int>(type: "int", nullable: false),
                    InterPercentage = table.Column<int>(type: "int", nullable: false),
                    DiplomaPercentage = table.Column<int>(type: "int", nullable: false),
                    GraduationPercentage = table.Column<int>(type: "int", nullable: false),
                    PostGraduationPercentage = table.Column<int>(type: "int", nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitHubProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CGPA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Certifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternshipExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtracurricularActivities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguagesKnown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredJobLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkAuthorization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPlaced = table.Column<bool>(type: "bit", nullable: false),
                    PlacedCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlacementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsData", x => x.StudentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsData");
        }
    }
}
