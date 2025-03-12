using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Model.VM
{
    public class MentorAllocationVM
    {
        public string SelectedSection { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMentorId { get; set; }
        public List<int> SelectedStudentIds { get; set; }

        public List<ApplicationUser> Mentors { get; set; }
        public List<Student> Students { get; set; }
    }
}
