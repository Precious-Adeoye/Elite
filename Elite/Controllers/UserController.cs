using Elite.Application.DTOs;
using Elite.Domain.Interface;
using Elite.Infrastructure.Data;
using Elite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
      private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UserController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }


        [HttpPost("Notification-preference")]
        public async Task<IActionResult> UpdateNotification([FromBody] Application.DTOs.NotificationPreferenceDTO dto)
        {
            var updated = await _userService.NotificationPreferenceAsync(dto.Email, dto.AllowNotifications);
            return updated
                ? Ok(new { Message = "Notification preference updated successfully." })
                : BadRequest("Failed to update notification preference.");
        }

        [HttpGet("resolved-account")]
        public async Task<IActionResult> ResolveAccount([FromQuery] string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest("Account number is required");

            if (accountNumber.Length != 10 || !accountNumber.All(char.IsDigit))
                return BadRequest("Invalid acct number.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == accountNumber);

                if (user == null)
                return NotFound("Account not found");

                return Ok(new
                {
                    accountName = user.FullName,
                    accountNumber = user.AccountNumber
                });
        }
    }
}
