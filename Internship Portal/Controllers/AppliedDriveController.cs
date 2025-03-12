using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Model.VM;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

            if (User.IsInRole(SD.Role_Student))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                query = query.Where(u => u.Student.UserId == userId);
            }

            if (User.IsInRole(SD.Role_TNP))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                query = query.Where(u => u.BlogPost.UserId == userId);
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

        [HttpPost]
        public IActionResult Export([FromBody] ExportFilterVM filters)
        {
            var appliedDrives = _unitOfWork.AppliedDrive.GetAll(
                filter: x =>
                    (string.IsNullOrEmpty(filters.Date) || x.AppliedOn.ToString("yyyy-MM-dd") == filters.Date) &&
                    (string.IsNullOrEmpty(filters.Company) || x.BlogPost.CompanyName.Contains(filters.Company)) &&
                    (string.IsNullOrEmpty(filters.Course) || x.Student.Course == filters.Course) &&
                    (!filters.Batch.HasValue || x.Student.Batch == filters.Batch),
                    //&&
                    //(filters.RollNumber || x.Student.RollNumber == filters.RollNumber),
                includeProperties: "BlogPost,Student"
            );
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("AppliedDrives");

                // Add headers
                worksheet.Cells[1, 1].Value = "Company Name";
                worksheet.Cells[1, 2].Value = "Student Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Roll Number";
                worksheet.Cells[1, 5].Value = "Year";
                worksheet.Cells[1, 6].Value = "Batch";
                worksheet.Cells[1, 7].Value = "Applied On";

                int row = 2;
                foreach (var drive in appliedDrives)
                {
                    worksheet.Cells[row, 1].Value = drive.BlogPost.CompanyName;
                    worksheet.Cells[row, 2].Value = drive.Student.Name;
                    worksheet.Cells[row, 3].Value = drive.Student.Email;
                    worksheet.Cells[row, 4].Value = drive.Student.RollNumber;
                    worksheet.Cells[row, 5].Value = drive.Student.Year;
                    worksheet.Cells[row, 6].Value = drive.Student.Batch;
                    worksheet.Cells[row, 7].Value = drive.AppliedOn.ToString("yyyy-MM-dd");
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                Response.Headers["Content-Disposition"] = "attachment; filename=AppliedDrives.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            }
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
