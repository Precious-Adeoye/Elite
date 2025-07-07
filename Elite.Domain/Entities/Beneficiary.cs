using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Domain.Entities
{
    public class Beneficiary
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }

        public User User { get; set; }

    }
    
    
}
