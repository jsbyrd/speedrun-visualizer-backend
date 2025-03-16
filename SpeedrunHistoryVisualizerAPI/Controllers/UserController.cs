using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Services;
using System.Security.Claims;

namespace SpeedrunHistoryVisualizerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserFromCookie()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return BadRequest("Could not find user.");

            User user = await userService.GetUserById(userId);
            if (user is null) return BadRequest("Could not find user.");

            var userResponse = new
            {
                user.Username,
                user.Role
            };

            return Ok(userResponse);
        }
    }
}
