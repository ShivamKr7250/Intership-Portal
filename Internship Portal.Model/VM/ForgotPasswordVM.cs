using System.ComponentModel.DataAnnotations;

namespace Internship_Portal.Model.VM
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
