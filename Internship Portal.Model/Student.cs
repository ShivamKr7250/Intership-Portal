using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship_Portal.Model
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public int RollNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public char Section { get; set; }

        [Required]
        public char Year { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public int Batch { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public int Backlogs { get; set; }

        [Required]
        public int MatricPercentage { get; set; }

        [Required]
        public int InterPercentage { get; set; }

        public int DiplomaPercentage { get; set; }

        public int GraduationPercentage { get; set; }

        public int PostGraduationPercentage { get; set; }

        [Required]
        public string ParentName { get; set; }

        [Required]
        public string ParentContact { get; set; }

        [Required]
        public string Skills { get; set; }

        [Required]
        public string Project { get; set; }

        [Required]
        public string GitHubProfile { get; set; }

        [Required]
        public string LinkedInProfile { get; set; }

        [Required]
        public string Resume { get; set; }

        // Additional Fields
        [Required]
        public DateTime DOB { get; set; }  // Date of Birth

        public string Gender { get; set; }  // Gender

        public string Nationality { get; set; }  // Nationality

        public decimal CGPA { get; set; }  // Cumulative GPA

        public string? Certifications { get; set; }  // Certifications (e.g., AWS, Azure)

        public string? InternshipExperience { get; set; }  // Past Internships

        public string? ExtracurricularActivities { get; set; }  // Clubs, leadership roles

        public string? LanguagesKnown { get; set; }  // Spoken and programming languages

        public string? PreferredJobLocation { get; set; }  // Preferred job location

        public string? WorkAuthorization { get; set; }  // Work status (e.g., visa status)

        public decimal ExpectedSalary { get; set; }  // Expected salary

        public bool IsPlaced { get; set; }  // Placement status

        public string? PlacedCompany { get; set; }  // Company where placed

        public DateTime? PlacementDate { get; set; }  // Date of placement

        public string? AdditionalNotes { get; set; }  // Any extra info
    }

}
