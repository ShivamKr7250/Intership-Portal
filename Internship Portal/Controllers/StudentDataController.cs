using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public IActionResult StudentDataRegistration(string userId)
        {
            if (userId == null)
            {
                //Create
                return View();
            }
            else
            {
                //Update
                Student studentData = _unitOfWork.StudentData.Get(u => u.UserId == userId);
                return View(studentData);
            }
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

            // Check if the student already exists by UserId
            var existingStudent = _unitOfWork.StudentData.Get(s => s.UserId == userId);

            if (existingStudent != null)
            {
                // Update the existing student record instead of inserting a new one
                existingStudent.RollNumber = obj.RollNumber;
                existingStudent.Name = obj.Name;
                existingStudent.Email = obj.Email;
                existingStudent.Contact = obj.Contact;
                existingStudent.Address = obj.Address;
                existingStudent.City = obj.City;
                existingStudent.State = obj.State;
                existingStudent.Section = obj.Section;
                existingStudent.Year = obj.Year;
                existingStudent.Department = obj.Department;
                existingStudent.Course = obj.Course;
                existingStudent.Specialization = obj.Specialization;
                existingStudent.Batch = obj.Batch;
                existingStudent.Backlogs = obj.Backlogs;
                existingStudent.MatricPercentage = obj.MatricPercentage;
                existingStudent.InterPercentage = obj.InterPercentage;
                existingStudent.DiplomaPercentage = obj.DiplomaPercentage;
                existingStudent.GraduationPercentage = obj.GraduationPercentage;
                existingStudent.PostGraduationPercentage = obj.PostGraduationPercentage;
                existingStudent.ParentName = obj.ParentName;
                existingStudent.ParentContact = obj.ParentContact;
                existingStudent.Skills = obj.Skills;
                existingStudent.Project = obj.Project;
                existingStudent.GitHubProfile = obj.GitHubProfile;
                existingStudent.LinkedInProfile = obj.LinkedInProfile;
                existingStudent.Resume = obj.Resume;
                existingStudent.DOB = obj.DOB;
                existingStudent.Gender = obj.Gender;
                existingStudent.Nationality = obj.Nationality;
                existingStudent.CGPA = obj.CGPA;
                existingStudent.Certifications = obj.Certifications;
                existingStudent.InternshipExperience = obj.InternshipExperience;
                existingStudent.ExtracurricularActivities = obj.ExtracurricularActivities;
                existingStudent.LanguagesKnown = obj.LanguagesKnown;
                existingStudent.PreferredJobLocation = obj.PreferredJobLocation;
                existingStudent.WorkAuthorization = obj.WorkAuthorization;
                existingStudent.ExpectedSalary = obj.ExpectedSalary;
                existingStudent.IsPlaced = obj.IsPlaced;
                existingStudent.PlacedCompany = obj.PlacedCompany;
                existingStudent.PlacementDate = obj.PlacementDate;
                existingStudent.AdditionalNotes = obj.AdditionalNotes;

                _unitOfWork.StudentData.Update(existingStudent);
                _unitOfWork.Save();

                TempData["success"] = "The Data has been Updated successfully.";
                return RedirectToAction("UserProfile", "Account");
            }

            // If no existing record, create a new one
            obj.UserId = userId;
            _unitOfWork.StudentData.Add(obj);
            _unitOfWork.Save();

            TempData["success"] = "The Registration has been done successfully.";
            return RedirectToAction("UserProfile", "Account");
        }

        #region API CALLS
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(char? year, bool? isPlaced, char? section, int? batch, string skills, int draw, int start, int length)
        {
            var query = _unitOfWork.StudentData.GetAll()
                .Select(student => new
                {
                    student.StudentId,
                    student.Name,
                    student.RollNumber,
                    student.Email,
                    student.Year,
                    student.Section,
                    student.Batch,
                    student.Skills,
                    student.IsPlaced,
                    Mentor = _unitOfWork.MentorAllocation.GetAll(null, null, false)
                        .Where(ma => ma.StudentId == student.StudentId)
                        .Select(ma => new
                        {
                            ma.Mentor,
                            MentorName = ma.Mentor.UserName, // Assuming Mentor has a Name property
                            MentorEmail = ma.Mentor.Email
                        }).FirstOrDefault() // Get only the first mentor allocation if multiple exist
                });

            // Apply filtering
            if (User.IsInRole(SD.Role_Student))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                query = query.Where(u => u.Email == userId);
            }
            if (year.HasValue)
            {
                query = query.Where(u => u.Year == year.Value);
            }
            if (isPlaced.HasValue)
            {
                query = query.Where(u => u.IsPlaced == isPlaced);
            }
            if (section.HasValue)
            {
                query = query.Where(u => u.Section == section.Value);
            }
            if (batch.HasValue)
            {
                query = query.Where(u => u.Batch == batch.Value);
            }
            if (!string.IsNullOrEmpty(skills))
            {
                var skillList = skills.Split(',').Select(s => s.Trim().ToLower()).ToList();
                query = query.Where(u => skillList.Any(skill => u.Skills.ToLower().Contains(skill)));
            }

            // Get total records count before applying pagination
            int totalRecords = query.Count();

            // Apply pagination
            var data = query.Skip(start).Take(length).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = data
            });
        }




        [HttpDelete]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Delete(int? id)
        {
            var StudentDataToBeDeleted = _unitOfWork.StudentData.Get(u => u.StudentId == id);
            if (StudentDataToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.StudentData.Remove(StudentDataToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successful" });
        }

        #endregion

    }
}
