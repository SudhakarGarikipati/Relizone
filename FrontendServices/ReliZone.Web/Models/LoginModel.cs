using System.ComponentModel.DataAnnotations;

namespace ReliZone.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage ="password must be minimum 6 characters long")]
        public string Password { get; set; }
    }
}
