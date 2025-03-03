using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Microsoft.AspNetCore.Authorization;
using Internship_Portal.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Internship_Portal.Model.VM;

namespace Internship_Portal.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {

            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public class LockUnlockRequest
        {
            public string Id { get; set; }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {

            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _unitOfWork.User.Get(u => u.Id == userId),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
            };

            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.User.Get(u => u.Id == userId))
                .GetAwaiter().GetResult().FirstOrDefault();
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagmentVM)
        {
            string oldRole = _userManager.GetRolesAsync(_unitOfWork.User.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id))
                .GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.User.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);

            if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
                //a role was updated

                //if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
                //{
                //    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                //}
                //if (oldRole == SD.Role_Company)
                //{
                //    applicationUser.CompanyId = null;
                //}
                _unitOfWork.User.Update(applicationUser);
                _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                //if (oldRole == SD.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
                //{
                //    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                //    _unitOfWork.ApplicationUser.Update(applicationUser);
                //    _unitOfWork.Save();
                //}
            }

            return RedirectToAction("Index ");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.User.GetAll().ToList();

            //foreach (var user in objUserList)
            //{
            //    user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

            //    if (user.Company == null)
            //    {
            //        user.Company = new Company() { Name = "" };
            //    }
            //}
            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] LockUnlockRequest request)
        {
            var objFromDb = _unitOfWork.User.Get(u => u.Id == request.Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.User.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Lock/Unlocking Successful" });
        }
        #endregion
    }
}
