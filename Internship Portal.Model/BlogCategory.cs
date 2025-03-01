using System.ComponentModel.DataAnnotations;

namespace Internship_Portal.Model
{
    public class BlogCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
