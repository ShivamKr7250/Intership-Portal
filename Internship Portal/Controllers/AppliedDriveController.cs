using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Internship_Portal.Controllers
{
    public class AppliedDriveController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public AppliedDriveController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var appliedDriveData = _unitOfWork.AppliedDrive.GetAll();
            return View(appliedDriveData);
        }

        #region API CALLS
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(DateTime? date, string company, string course, int? batch, int? rollNumber, int draw, int start, int length)
        {
            IEnumerable<AppliedDrive> query;
            query = _unitOfWork.AppliedDrive.GetAll(includeProperties: "BlogPost,Student");

            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                query = query.Where(u => u.Student.UserId == userId);
            }

            // Apply filtering
            if (date.HasValue)
            {
                query = query.Where(u => u.AppliedOn.Date == date.Value.Date);
            }
            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(u => u.BlogPost.CompanyName.ToLower() == company.ToLower());
            }
            if (!string.IsNullOrEmpty(course))
            {
                query = query.Where(u => u.Student.Course.ToLower() == course.ToLower());
            }
            
            if (batch.HasValue)
            {
                query = query.Where(u => u.Student.Batch == batch.Value);
            }
            if (rollNumber.HasValue)
            {
                query = query.Where(u => u.Student.RollNumber == rollNumber.Value);
            }

            // Get total records count before applying pagination
            int totalRecords = query.Count();

            // Select only necessary fields & apply pagination
            var data = query
                .Select(a => new
                {
                    blogPost = new { companyName = a.BlogPost.CompanyName },
                    student = new
                    {
                        name = a.Student.Name,
                        email = a.Student.Email,
                        rollNumber = a.Student.RollNumber,
                        year = a.Student.Year,
                        batch = a.Student.Batch
                    },
                    appliedOn = a.AppliedOn
                })
                .Skip(start)
                .Take(length)
                .ToList();

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
            var appliedDriveToBeDeleted = _unitOfWork.AppliedDrive.Get(u => u.Id == id);
            if (appliedDriveToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.AppliedDrive.Remove(appliedDriveToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successful" });
        }

        #endregion
    }
}
