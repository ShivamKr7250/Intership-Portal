using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Model
{
    public class AppliedDrive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DriveId { get; set; }

        [ForeignKey("DriveId")]
        public BlogPost? BlogPost { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    }
}
