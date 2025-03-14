﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Internship_Portal.Model.VM
{
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
