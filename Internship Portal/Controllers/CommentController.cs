using Microsoft.AspNetCore.Mvc;
using Internship_Portal.Data_Access.Repository.IRepository;

namespace Internship_Portal.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int id)
        {
            return RedirectToAction("Index" );
        }
    }
}
