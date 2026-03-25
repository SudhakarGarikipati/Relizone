using System.ComponentModel.DataAnnotations;

namespace ReliZone.Web.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage ="Name is required")]
        [StringLength(50, ErrorMessage = "Name must be a maximum of 50 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "password must be minimum 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role must be a maximum of 50 characters long.")]
        public string Role { get; set; }
    }
}
