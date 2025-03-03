using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Internship_Portal.Controllers
{
    public class StudentDataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var studentData = _unitOfWork.StudentData.GetAll();
            return View(studentData);
        }

        public IActionResult StudentDataRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StudentDataRegistration(Student obj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("User not authenticated.");
            }

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Check if the student already exists
            var existingStudent = _unitOfWork.StudentData.Get(s => s.Email == obj.Email);

            if (existingStudent != null)
            {
                TempData["error"] = "Student data already exists!";
                return RedirectToAction("StudentRegistrationForm", "Student");  // Redirect back to form
            }

            // Creating a new Student object
            Student student = new Student
            {
                RollNumber = obj.RollNumber,
                Name = obj.Name,
                Email = obj.Email,
                Contact = obj.Contact,
                Address = obj.Address,
                City = obj.City,
                State = obj.State,
                Section = obj.Section,
                Year = obj.Year,
                Department = obj.Department,
                Course = obj.Course,
                Specialization = obj.Specialization,
                Batch = obj.Batch,
                Backlogs = obj.Backlogs,
                MatricPercentage = obj.MatricPercentage,
                InterPercentage = obj.InterPercentage,
                DiplomaPercentage = obj.DiplomaPercentage,
                GraduationPercentage = obj.GraduationPercentage,
                PostGraduationPercentage = obj.PostGraduationPercentage,
                ParentName = obj.ParentName,
                ParentContact = obj.ParentContact,
                Skills = obj.Skills,
                Project = obj.Project,
                GitHubProfile = obj.GitHubProfile,
                LinkedInProfile = obj.LinkedInProfile,
                Resume = obj.Resume,
                DOB = obj.DOB,
                Gender = obj.Gender,
                Nationality = obj.Nationality,
                CGPA = obj.CGPA,
                Certifications = obj.Certifications,
                InternshipExperience = obj.InternshipExperience,
                ExtracurricularActivities = obj.ExtracurricularActivities,
                LanguagesKnown = obj.LanguagesKnown,
                PreferredJobLocation = obj.PreferredJobLocation,
                WorkAuthorization = obj.WorkAuthorization,
                ExpectedSalary = obj.ExpectedSalary,
                IsPlaced = obj.IsPlaced,
                PlacedCompany = obj.PlacedCompany,
                PlacementDate = obj.PlacementDate,
                AdditionalNotes = obj.AdditionalNotes,
            };

            // Add student to the database
            _unitOfWork.StudentData.Add(student);
            _unitOfWork.Save();

            TempData["success"] = "The Registration has been done successfully.";
            return RedirectToAction("RedirectPage", "Student", new { registrationId = student.StudentId });
        }

    }
}
