using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Model.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Upsert()
        {
            var mentors = _userManager.GetUsersInRoleAsync("Mentor").GetAwaiter().GetResult(); // Forcing sync execution

            var model = new MentorAllocationVM
            {
                Mentors = mentors.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.UserName // Use `m.Name` if applicable
                }).ToList(),

                Students = _unitOfWork.StudentData.GetAll().Select(s => new SelectListItem
                {
                    Value = s.StudentId.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Upsert(MentorAllocationVM model)
        {
            if (model.SelectedMentorId == 0 || model.SelectedStudentIds == null || !model.SelectedStudentIds.Any())
            {
                TempData["error"] = "Please select a mentor and at least one student.";
                return RedirectToAction("Index");
            }

            foreach (var studentId in model.SelectedStudentIds)
            {
                var student = _unitOfWork.StudentData.Get(studentId);
                if (student != null)
                {
                    student. = model.SelectedMentorId;
                }
            }

            _dbContext.SaveChanges();
            TempData["success"] = "Mentor allocated successfully.";
            return RedirectToAction("Index");
        }
    }
}
