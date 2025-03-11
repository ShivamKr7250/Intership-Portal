using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship_Portal.Model
{
    public class BlogPost
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public BlogCategory? BlogCategory { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(400)]
        public string ShortDescription { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        [Display(Name = "Blog Thumbnail")]
        public string BlogThumbnail { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Tags { get; set; }

        [NotMapped]
        public string AuthorName { get; set; }

        // Eligibility Criteria
        public int Batch { get; set; }

        [StringLength(100)]
        public string Course { get; set; }

        public int? MinimumCGPA { get; set; }
        public int? MinimumMatricPercentage { get; set; }
        public int? MinimumInterPercentage { get; set; }
        public int? MinimumGraduationPercentage { get; set; }
        public int? MaximumPostGraduationCGPA { get; set; }

        public DateTime? ApplicationDeadline { get; set; }

        public bool HasYearGap { get; set; }  // Changed from string to boolean

        // Navigation Properties
        public ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
    }
}
