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

        public async Task<IActionResult> Upsert()
        {
            // ✅ Get all mentors asynchronously
            var mentors = await _userManager.GetUsersInRoleAsync("Mentor");

            // ✅ Get all students asynchronously
            var students =  _unitOfWork.StudentData.GetAll(); // Ensure GetAllAsync() exists

            // ✅ ViewModel with select lists
            var model = new MentorAllocationVM
            {
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



        //[HttpPost]
        //public IActionResult Upsert(MentorAllocationVM model)
        //{
        //    if (model.SelectedMentorId == 0 || model.SelectedStudentIds == null || !model.SelectedStudentIds.Any())
        //    {
        //        TempData["error"] = "Please select a mentor and at least one student.";
        //        return RedirectToAction("Index");
        //    }

        //    foreach (var studentId in model.SelectedStudentIds)
        //    {
        //        var student = _unitOfWork.StudentData.Get(studentId);
        //        if (student != null)
        //        {
        //            student. = model.SelectedMentorId;
        //        }
        //    }

        //    _dbContext.SaveChanges();
        //    TempData["success"] = "Mentor allocated successfully.";
        //    return RedirectToAction("Index");
        //}
    }
}
