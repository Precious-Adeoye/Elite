using Microsoft.AspNetCore.Identity;

namespace Elite.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {

        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Bvn { get; set; }
        public string Pin { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; } = 0;
        public decimal LoanBalance { get; set; } = 0;
        public string ProfileImageUrl { get; set; } = "";
        public bool IsNotificationAllowed { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
