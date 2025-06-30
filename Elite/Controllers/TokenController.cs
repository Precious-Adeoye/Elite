using Elite.Application.DTOs;
using Elite.Application.Services;
using Elite.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elite.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public TokenController(UserManager<User> userManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized("Invalid email or password.");
            }
            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                Message = "Login successful.",
                User = new
                {
                    user.Email,
                    user.FullName,
                    user.PhoneNumber,
                    user.AccountNumber,
                    user.AccountBalance,
                    user.LoanBalance
                }
            });
        }
    }
}
