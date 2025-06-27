using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elite.Domain.Entities;
using Elite.Application.DTOs;



namespace Elite.Domain.Interface
{
    public interface IUserService
    {
        string RequestOtp(string email);
        bool VerifyOtp(string email, string otp);
        bool SetBvn(string email, string bvn);
        bool SetUserInfo(RegisterDto dto); 
        bool SetPin(string email, string pin);
        Task<bool> ConfirmPinAync(string email, string confirmPin);
        Task<User> ValidateLoginAsync(string email, string password);
        Task<bool> NotificationPreferenceAsync(string email, bool isAllowed);
    }
}
