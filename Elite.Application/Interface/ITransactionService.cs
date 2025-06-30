using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elite.Application.DTOs;


namespace Elite.Domain.Interface
{
    public interface ITransactionService
    {
        Task AddMoneyAsync(Guid userId, decimal amount, string description);
        Task SendMoneyAsync(Guid userId, string receiverAccountNumber ,decimal amount, string description, bool isExternal = false, string bankCode = null);
      
    }
}
