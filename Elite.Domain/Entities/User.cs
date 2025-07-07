using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Elite.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The phone number must be 11 digits")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "BVN must be 11 digits")]
        public string Bvn { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; } = 0;
        public decimal LoanBalance { get; set; } = 0;
        public string ProfileImageUrl { get; set; } = "";
        public bool IsNotificationAllowed { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

    }
}
