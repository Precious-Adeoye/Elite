using System.ComponentModel.DataAnnotations;

namespace Elite.DTOs

{
    public class BvnDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Bvn { get; set; }
    }

    
}
