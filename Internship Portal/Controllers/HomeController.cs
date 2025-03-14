using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Models;
using Internship_Portal.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace Internship_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Contact obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Contact.Add(obj);
                _unitOfWork.Save();
                TempData["MessageSent"] = true;
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult Contact()
        {
            var contact = _unitOfWork.Contact.GetAll();
            return View(contact);
        }

        public IActionResult Internship()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Contact(Contact obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Contact.Add(obj);
        //        _unitOfWork.Save();
        //        ViewBag.Message = "Your message has been sent. Thank you!";
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else { 
        //        return View();
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API Calls
        [HttpGet]
        public IActionResult DisplayCounter()
        {
            var objRegistration = _unitOfWork.RegistrationForm.GetAll();

            // Replace 'Domain1', 'Domain2', etc. with your actual domain values
            var domainCounts = new
            {
                Domain1 = objRegistration.Count(r => r.Domain == "Full Stack"),
                Domain2 = objRegistration.Count(r => r.Domain == "Frontend"),
                Domain3 = objRegistration.Count(r => r.Domain == "Backend"),
                Domain4 = objRegistration.Count(r => r.Domain == "Mobile Applications")
            };

            return Json(new { data = domainCounts });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll(Contact obj)
        {
            IEnumerable<Contact> objRegistration;
            objRegistration = _unitOfWork.Contact.GetAll();
            return Json(new { data = objRegistration });
        }

        #endregion
    }
}
