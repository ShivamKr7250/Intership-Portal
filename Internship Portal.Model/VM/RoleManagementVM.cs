using Internship_Portal.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RF_Technologies.Model.VM
{
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
