using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Authorization;
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
            var existingStudent = _unitOfWork.StudentData.Get(s => s.Email == obj.Email || s.RollNumber == obj.RollNumber);

            if (existingStudent != null)
            {
                TempData["error"] = "Student data already exists!";
                return View(obj);  // Redirect back to form
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
            return RedirectToAction("UserProfile", "Account");
        }

        #region API CALLS
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status, int? year, bool? isPlaced, char? section, int? batch, string skills)
        {
            IEnumerable<Student> objRegistration;

            if (User.IsInRole(SD.Role_Admin))
            {
                objRegistration = _unitOfWork.StudentData.GetAll();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objRegistration = _unitOfWork.StudentData.GetAll(u => u.Email == userId ); // Adjust condition based on your logic
            }

            // Apply filtering
            if (year.HasValue)
            {
                objRegistration = objRegistration.Where(u => u.Year == year.Value);
            }
            if (isPlaced.HasValue)
            {
                objRegistration = objRegistration.Where(u => u.IsPlaced == isPlaced.Value);
            }
            if (section.HasValue)
            {
                objRegistration = objRegistration.Where(u => u.Section == section.Value);
            }
            if (batch.HasValue)
            {
                objRegistration = objRegistration.Where(u => u.Batch == batch.Value);
            }
            if (!string.IsNullOrEmpty(skills))
            {
                var skillList = skills.Split(',').Select(s => s.Trim().ToLower()).ToList();
                objRegistration = objRegistration.Where(u => skillList.Any(skill => u.Skills.ToLower().Contains(skill)));
            }

            return Json(new { data = objRegistration });
        }


        [HttpDelete]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Delete(int? id)
        {
            var RegistrationToBeDeleted = _unitOfWork.RegistrationForm.Get(u => u.ID == id);
            if (RegistrationToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.RegistrationForm.Remove(RegistrationToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successful" });
        }

        #endregion

    }
}
