using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Model.VM
{
    public class MentorAllocationVM
    {
        [Required]
        public string SelectedSection { get; set; } = string.Empty; // Prevents null issues

        [Required]
        [Range(1, 10, ErrorMessage = "Please enter a valid year.")]
        public int SelectedYear { get; set; }

        [Required]
        public string SelectedMentorId { get; set; } // Changed from int to string for Identity

        [Required]
        public List<int> SelectedStudentIds { get; set; } = new List<int>(); // Ensures no null issues

        public MentorAllocation? MentorAllocation { get; set; }

        public IEnumerable<ApplicationUser> Mentors { get; set; } = new List<ApplicationUser>();
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
    }

}
