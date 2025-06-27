using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Domain.Entities
{
    public class UserRegistration
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string OtpCode { get; set; }
        public DateTime OtpGeneratedAt { get; set; }
        public bool IsOtpVerified { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Country { get; set; }
        public string? Bvn { get; set; }
        public string? Pin { get; set; }
        public bool IsPinConfirmed { get; set; }

        public bool IsFullyRegistered =>
            IsOtpVerified &&
            IsPinConfirmed &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(PhoneNumber) &&
            !string.IsNullOrWhiteSpace(Bvn);
    }
}
