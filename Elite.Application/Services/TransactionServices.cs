using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elite.Application.DTOs;
using Elite.Domain.Entities;
using Elite.Domain.Interface;
using Elite.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Elite.Application.Services
{
    public class TransactionServices : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMoneyAsync(Guid userId, decimal amount, string description)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                throw new Exception("User not found");
            }

            user.AccountBalance += amount;
            _context.Transactions.Add(new Transaction
            {
                UserId = userId,
                TransactionType = "Deposit",
                Amount = amount,
                Description = description,
                Date = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
        }

        async Task ITransactionService.SendMoneyAsync(Guid senderId, string receiverAccountNumber, decimal amount, string description, bool isExternal, string bankCode)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
                throw new Exception("Sender not found");

            if (sender.AccountBalance < amount)
            {
                throw new Exception("Insufficient funds");
            }

            if(isExternal)
            {
                sender.AccountBalance = amount;

                _context.Transactions.Add(new Transaction
                {
                    UserId = senderId,
                    TransactionType = "External Transfar",
                    Amount = amount,
                    Description = $"Transferred to {receiverAccountNumber} via {bankCode}. Details: {description}",
                    Date = DateTime.UtcNow,
                });
                await _context.SaveChangesAsync();
                return;
            }

            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == receiverAccountNumber);
            if (receiver == null)
                throw new Exception("Receiver not found");

            sender.AccountBalance -= amount;
            receiver.AccountBalance += amount;

            _context.Transactions.Add(new Transaction
            {
                UserId = senderId,
                TransactionType = "Transfer",
                Amount = amount,
                Description = $"Sent to {receiver.FullName} - {description}",
                Date = DateTime.UtcNow,
            });
            _context.Transactions.Add(new Transaction
            {
                UserId = receiver.Id,
                TransactionType = "Received",
                Amount = amount,
                Description = $"Received from {sender.FullName} - {description}",
                Date = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
        }
    }
}
