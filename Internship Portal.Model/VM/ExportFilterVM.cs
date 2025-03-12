using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Model.VM
{
    public class ExportFilterVM
    {
        public string? Date { get; set; }
        public string? Company { get; set; }
        public string? Course { get; set; }
        public int? Batch { get; set; }
        public string? RollNumber { get; set; }
    }
}
