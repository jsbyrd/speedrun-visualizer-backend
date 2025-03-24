using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedrunHistoryVisualizerAPI.Data;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;
using SpeedrunHistoryVisualizerAPI.Services;
using System.Security.Claims;

namespace SpeedrunHistoryVisualizerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, AppDbContext context) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserDto request)
        {
            bool successfulLogin = await authService.LoginAsync(request);
            if (!successfulLogin) return BadRequest("Invalid username or password.");
            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserDto request)
        {
            bool successfulRegister = await authService.RegisterAsync(request);
            if (!successfulRegister) return BadRequest("Username already exists.");
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens()
        {
            bool success = await authService.RefreshTokensAsync();
            if (!success) return Unauthorized();
            return Ok(new { message = "Token refreshed successfully" });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return BadRequest("Could not log out.");
            authService.LogoutAsync(userId);
            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AuthenticatedOnlyEndpoint()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return BadRequest("Could not find user.");

            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user is null) return BadRequest("Could not find user.");

            return Ok($"You are authenticated, {user.Username}");
        }
    }
}
