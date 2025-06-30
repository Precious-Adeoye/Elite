using Elite.Application.DTOs;
using Elite.Domain.Interface;
using Elite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
      private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Notification-preference")]
        public async Task<IActionResult> UpdateNotification([FromBody] Application.DTOs.NotificationPreferenceDTO dto)
        {
            var updated = await _userService.NotificationPreferenceAsync(dto.Email, dto.AllowNotifications);
            return updated
                ? Ok(new { Message = "Notification preference updated successfully." })
                : BadRequest("Failed to update notification preference.");
        }

      
    }
}
