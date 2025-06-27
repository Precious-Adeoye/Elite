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

        public async Task SendMoneyAsync(Guid userId, string receiverAccountNumber, decimal amount, string description)
        {
           var sender = await _context.Users.FindAsync(userId);
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == receiverAccountNumber);

            if (sender == null || receiver == null)
            {
                throw new Exception("Sender or receiver not found");
            }

            if (sender.AccountBalance < amount)
            {
                throw new Exception("Insufficient balance");
            }
            sender.AccountBalance -= amount;
            receiver.AccountBalance += amount;

            _context.Transactions.Add(new Transaction
            {
                UserId = userId,
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
