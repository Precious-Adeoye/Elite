using System.ComponentModel.DataAnnotations;

namespace Elite.DTOs
{
    public class RequestOtpDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
