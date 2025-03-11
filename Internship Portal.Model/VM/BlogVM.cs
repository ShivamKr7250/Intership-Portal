using Microsoft.AspNetCore.Mvc.Rendering;

namespace Internship_Portal.Model.VM
{
    public class BlogVM
    {
        public BlogPost BlogPost { get; set; }
        public IEnumerable<BlogPost> Post {  get; set; }
        public BlogComment BlogComment { get; set; }
        public Student Student { get; set; }
        public ApplicationUser? User { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public List<string>? Tags { get; set; }
        public IEnumerable<BlogComment>? Comments { get; set; }
        public bool HasAlreadyApplied { get; set; }
    }
}
