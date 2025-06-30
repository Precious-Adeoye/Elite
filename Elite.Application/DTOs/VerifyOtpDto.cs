using System.ComponentModel.DataAnnotations;

namespace Elite.DTOs
{
    public class VerifyOtpDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Must input OTP")]
        public string Otp { get; set; }
    }
}
