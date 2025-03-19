using Internship_Portal.Data_Access.Repository;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Model.VM;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Internship_Portal.Controllers
{
    public class MentorAllocationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public MentorAllocationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var mentorData = _unitOfWork.MentorAllocation.GetAll();
            return View(mentorData);
        }

        public async Task<IActionResult> Upsert()
        {
            // ✅ Get all mentors asynchronously
            var mentors = await _userManager.GetUsersInRoleAsync("Mentor");

            // ✅ Get all students asynchronously
            var students =  _unitOfWork.StudentData.GetAll(); // Ensure GetAllAsync() exists

            // ✅ ViewModel with select lists
            var model = new MentorAllocationVM
            {
                MentorAllocation = new MentorAllocation(),
                Mentors = mentors,
                Students = students
            };

            // ✅ Passing SelectList to ViewBag
            ViewBag.MentorList = mentors.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.UserName // Use `m.Name` if applicable
            }).ToList();

            ViewBag.StudentList = students.Select(s => new SelectListItem
            {
                Value = s.StudentId.ToString(),
                Text = s.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Upsert(MentorAllocationVM model)
        {
            if (model.SelectedStudentIds == null || !model.SelectedStudentIds.Any())
            {
                TempData["error"] = "Please select a mentor and at least one student.";
                return RedirectToAction("Index");
            }

            foreach (var studentId in model.SelectedStudentIds)
            {
                var existingAllocation = _unitOfWork.MentorAllocation.Get(m => m.StudentId == studentId);

                if (existingAllocation != null)
                {
                    // Update existing allocation
                    existingAllocation.AllocatedOn = DateTime.UtcNow;
                    existingAllocation.UserId = model.SelectedMentorId;
                    _unitOfWork.MentorAllocation.Update(existingAllocation);
                }
                else
                {
                    // Insert new allocation
                    var newAllocation = new MentorAllocation
                    {
                        StudentId = studentId,
                        UserId = model.SelectedMentorId,
                        AllocatedOn = DateTime.UtcNow
                    };
                    _unitOfWork.MentorAllocation.Add(newAllocation);
                }
            }

            _unitOfWork.Save();
            TempData["success"] = "Mentor allocated successfully.";
            return RedirectToAction("Index");
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetStudents(char section, char year)
        {
            string sectionString = section.ToString();  // Convert char to string

            var students = _unitOfWork.StudentData.GetAll()
                .Where(u => u.Year == year && u.Section.ToString() == sectionString)
                .ToList();  // Execute query

            return Ok(students);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll(char? year, bool? isPlaced, char? section, int? batch, string skills, int draw, int start, int length)
        {
            var mentorAllocations = _unitOfWork.MentorAllocation.GetAll()
                .Where(ma => ma.Mentor != null) // Ensure mentors exist
                .Select(ma => new
                {
                    MentorId = ma.Mentor.Id,
                    MentorName = ma.Mentor.UserName,
                    MentorEmail = ma.Mentor.Email,
                    StudentYear = ma.Student != null ? ma.Student.Year : (char?)null,
                    StudentSection = ma.Student != null ? ma.Student.Section : (char?)null,
                    StudentBatch = ma.Student != null ? ma.Student.Batch : (int?)null,
                    StudentId = ma.Student != null ? ma.Student.StudentId : (int?)null
                });

            // ✅ Apply Filters
            if (year.HasValue)
            {
                mentorAllocations = mentorAllocations.Where(u => u.StudentYear == year.Value);
            }
            if (batch.HasValue)
            {
                mentorAllocations = mentorAllocations.Where(u => u.StudentBatch == batch.Value);
            }

            // ✅ Aggregate Data: Group by Mentor
            var mentorGroupedData = mentorAllocations
                .GroupBy(m => new { m.MentorId, m.MentorName, m.MentorEmail })
                .Select(group => new
                {
                    MentorId = group.Key.MentorId,
                    MentorName = group.Key.MentorName,
                    MentorEmail = group.Key.MentorEmail,
                    TotalAllocatedStudents = group.Count(m => m.StudentId != null), // Count only valid students
                    Years = group.Where(m => m.StudentYear.HasValue).Select(m => m.StudentYear).Distinct().ToList(),
                    Batches = group.Where(m => m.StudentBatch.HasValue).Select(m => m.StudentBatch).Distinct().ToList(),
                    Sections = group.Where(m => m.StudentSection.HasValue).Select(m => m.StudentSection) .Distinct().ToList()
                }).ToList();

            // ✅ Get total records count BEFORE pagination
            int totalRecords = mentorGroupedData.Count();

            // ✅ Apply Pagination
            var data = mentorGroupedData.Skip(start).Take(length).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = data
            });
        }




        #endregion

    }
}
