using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Model
{
    public class MentorAllocation
    {
        [Key]
        public int MentorAllocationId { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? Mentor { get; set; }

        // ✅ Student (For Single Allocation)
        public int? StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        // ✅ Allocation Timestamp
        public DateTime AllocatedOn { get; set; } = DateTime.UtcNow;

    }
}
