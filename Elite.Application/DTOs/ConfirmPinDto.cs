using System.ComponentModel.DataAnnotations;

namespace Elite.DTOs
{
    public class ConfirmPinDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be 4 digits")]
        public string ConfirmedPin { get; set; }
    }
}
