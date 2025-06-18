using System.Security.Cryptography.X509Certificates;
using Elite.DTOs;
using Elite.Model;

namespace Elite.Services
{
    public class UserServices
    {
        private readonly List<TempUserData> _tempUsers = new();
        private readonly List<User> _registeredUsers = new();

        public string RequestOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            var existing = _tempUsers.FirstOrDefault(u => u.Email == email);
            if (existing != null)
            {
                existing.OtpCode = otp;
                existing.OtpGeneratedAt = DateTime.UtcNow;
                existing.IsOtpVerified = false;
            }
            else
            {
                _tempUsers.Add(new TempUserData
                {
                    Email = email,
                    OtpCode = otp,
                    OtpGeneratedAt = DateTime.UtcNow,
                    IsOtpVerified = false
                });
            }

            Console.WriteLine($"[OTP] sent to {email} : {otp}");
            return otp;

        }

        public bool VerifyOtp(string email, string otp)
        {
            var user = _tempUsers.FirstOrDefault(u => u.Email == email);
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
            return true;
        }

        public bool SetBvn (string email, string bvn)
        {
            var user = _tempUsers.FirstOrDefault(u => u.Email == email && u.IsOtpVerified);
            if (user == null || !user.IsOtpVerified)
            {
                return false;
            }
            user.Bvn = bvn;
            return true;
        }

        public bool SetUserInfo(RegisterDto dto)
        {
            var user = _tempUsers.FirstOrDefault(u => u.Email == dto.Email && u.IsOtpVerified);
            if (user == null || string.IsNullOrWhiteSpace(dto.Password) || dto.Password != dto.ConfirmPassWord)
            {
                return false;
            }
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Password = dto.Password;
            
            
            return true;
        }

        public bool SetPin(string email, string pin)
        {
            var user = _tempUsers.FirstOrDefault(u => u.Email == email && u.IsOtpVerified);
            if (user == null || string.IsNullOrWhiteSpace(pin))
            {
                return false;
            }
            user.Pin = pin;
            return true;
        }

        public bool ConfirmPin(string email, string confirmedPin)
        {
            var user = _tempUsers.FirstOrDefault(u => u.Email == email && u.IsOtpVerified);
            if (user == null || user.Pin != confirmedPin) 
            {
                return false;
            }
            user.IsPinConfirmed = true;
            if (user.IsFullyRegistered)
            {
                _registeredUsers.Add(new User
                {
                    Email = user.Email,
                    Username = user.Username,
                    FullName = $"{user.FirstName} {user.LastName}",
                    PhoneNumber = user.PhoneNumber,
                    Password = user.Password,
                    Bvn = user.Bvn,
                    Pin = user.Pin
                });
                _tempUsers.Remove(user);
                return true;
            }
            return false;
        }

        public User ValidateLogin(string email, string password)
        {
            var user = _registeredUsers.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
