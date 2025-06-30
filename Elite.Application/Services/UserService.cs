using System.Security.Cryptography.X509Certificates;
using Elite.Application.DTOs;
using Elite.Domain.Entities;
using Elite.Domain.Interface;
using Elite.DTOs;
using Elite.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Elite.Services
{
    public class UserService : IUserService
    {
      private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string RequestOtp(string email)
        {
            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var existing = _context.UserRegistrations.FirstOrDefault(u => u.Email == email);
                if (existing != null)
                {
                    existing.OtpCode = otp;
                    existing.OtpGeneratedAt = DateTime.UtcNow;
                    existing.IsOtpVerified = false;
                }
                else
                {
                    _context.UserRegistrations.Add(new UserRegistration
                    {
                        Email = email,
                        OtpCode = otp,
                        OtpGeneratedAt = DateTime.UtcNow,
                        IsOtpVerified = false
                    });
                }
                _context.SaveChanges();
                Console.WriteLine($"[OTP] sent to {email} : {otp}");
                return otp;

            }catch(Exception ex)
            {
                Console.WriteLine("An error occurred while generating OTP:");
                Console.WriteLine(ex.Message);
               
                Console.WriteLine(ex.StackTrace);

               
                throw;
            }



        }

        public bool VerifyOtp(string email, string otp)
        {
            var user = _context.UserRegistrations.FirstOrDefault(u => u.Email == email);
            if ( user == null || user.OtpCode != otp)
            {
                return false;
            }

            var elaspsed = DateTime.UtcNow - user.OtpGeneratedAt;
            if (elaspsed.TotalMinutes > 5)
            {
                Console.WriteLine($"[OTP] expired for {email}");
                return false;
            }
           
            user.IsOtpVerified = true;
            _context.SaveChanges();
            return true;
        }

        public bool SetBvn (string email, string bvn)
        {
            var user = _context.UserRegistrations.FirstOrDefault(u => u.Email == email && u.IsOtpVerified);
            if (user == null || !user.IsOtpVerified)
            {
                Console.WriteLine("User not found");
                return false;
            }
           if (!user.IsOtpVerified)
            {
                Console.WriteLine("OTP not verified");
                return false;
            }
            user.Bvn = bvn;
            _context.SaveChanges();
            return true;
        }

        public bool SetUserInfo(RegisterDto dto)
        {
            var user = _context.UserRegistrations.FirstOrDefault(u => u.Email == dto.Email && u.IsOtpVerified);
            if (user == null || string.IsNullOrWhiteSpace(dto.Password) || dto.Password != dto.ConfirmPassWord)
            {
                return false;
            }
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Password = dto.Password;

            _context.SaveChanges();
            return true;
        }

        public bool SetPin(string email, string pin)
        {
            var user = _context.UserRegistrations.FirstOrDefault(u => u.Email == email && u.IsOtpVerified);
            if (user == null || string.IsNullOrWhiteSpace(pin))
            {
                return false;
            }
           
            user.Pin = pin;
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> ConfirmPinAync(string email, string confirmedPin)
        {
            var session = await _context.UserRegistrations.FirstOrDefaultAsync(u => u.Email == email && u.IsOtpVerified);
            if (session == null || session.Pin != confirmedPin) 
            {
                return false;
            }

            session.IsPinConfirmed = true;
            if (session.IsFullyRegistered)
            {
                var user = new User
                {
                    Email = session.Email,
                    UserName = session.Email,
                    FullName = $"{session.FirstName} {session.LastName}",
                    PhoneNumber = session.PhoneNumber,
                    Bvn = session.Bvn,
                    Pin = session.Pin,
                    Country = session.Country,
                    IsNotificationAllowed = false
                };
                var result = await _userManager.CreateAsync(user, session.Password);
                if (result.Succeeded)
                {
                    _context.UserRegistrations.Remove(session);
                    return true;
                }
                return false;
            }
         await _context.SaveChangesAsync();
            return true;
        }

        public async Task <User> ValidateLoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)   return null;
            
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            _context.SaveChanges();
            return isPasswordValid ? user : null;
        }

        public  async Task<bool> NotificationPreferenceAsync(string email, bool isAllowed)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            user.IsNotificationAllowed = isAllowed;
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
            return true;
        }

    }
}
