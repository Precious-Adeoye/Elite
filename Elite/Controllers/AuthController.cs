using Elite.DTOs;
using Elite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserServices _userServices;

        public AuthController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("request-otp")]
        public IActionResult RequestOtp([FromBody] RequestOtpDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
            {
                return BadRequest("Email is required.");
            }
            var otp = _userServices.RequestOtp(dto.Email);
            return Ok(new { Otp = otp, Message = "OTP sent successfully." });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            if (string.IsNullOrEmpty(dto.Otp))
            {
                return BadRequest(" OTP are required.");
            }
            var isVerified = _userServices.VerifyOtp(dto.Email, dto.Otp);
            if (!isVerified)
            {
                return BadRequest("Invalid OTP or OTP expired.");
            }
            return Ok(new { Message = "OTP verified successfully." });
        }

        [HttpPost("bvn")]
        public IActionResult SetBvn([FromBody] BvnDto dto)
        {
            if (string.IsNullOrEmpty(dto.Bvn))
            {
                return BadRequest(" BVN are required.");
            }
            var isSet = _userServices.SetBvn(dto.Email, dto.Bvn);
            if (!isSet)
            {
                return BadRequest("BVN could not be set. Ensure OTP is verified.");
            }
            return Ok(new { Message = "BVN set successfully." });
        }

        [HttpGet("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var users = _userServices.SetUserInfo(dto);
            return users ? Ok(users) : BadRequest("Failed to set Users");
        }

        [HttpGet("pin")]
        public IActionResult SetPin([FromBody] CreatePinDto dto)
        {
            if (string.IsNullOrEmpty(dto.Pin))
            {
                return BadRequest(" Pin are required.");
            }
            var isSet = _userServices.SetPin(dto.Email, dto.Pin);
            if (!isSet)
            {
                return BadRequest("Pin could not be set. Ensure OTP is verified.");
            }
            return Ok(new { Message = "Pin set successfully." });
        }

        [HttpGet("confirm-pin")]
        public IActionResult ConfirmPin([FromBody] ConfirmPinDto dto)
        {
            if (string.IsNullOrEmpty(dto.ConfirmedPin))
            {
                return BadRequest(" Pin are required.");
            }
            var isConfirmed = _userServices.ConfirmPin(dto.Email, dto.ConfirmedPin);
            if (!isConfirmed)
            {
                return BadRequest("Pin could not be confirmed. Ensure OTP is verified.");
            }
            return Ok(new { Message = "Registration completed" });
        }
    }

}
