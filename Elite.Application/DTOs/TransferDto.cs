using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Application.DTOs
{
    public class TransferDto
    {
        public Guid senderId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsExternal {  get; set; }
        public string BankCode { get; set; }
    }
}
