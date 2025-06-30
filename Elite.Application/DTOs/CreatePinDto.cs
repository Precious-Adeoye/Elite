using System.ComponentModel.DataAnnotations;

namespace Elite.DTOs
{
    public class CreatePinDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be 4 digits")]
        public string Pin { get; set; }
    }
}
