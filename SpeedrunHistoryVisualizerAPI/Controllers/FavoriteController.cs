using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedrunHistoryVisualizerAPI.Models;
using SpeedrunHistoryVisualizerAPI.Services;
using System.Security.Claims;

namespace SpeedrunHistoryVisualizerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController(IFavoriteService favoriteService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFavorite([FromBody] FavoriteDto favoriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            var createdFavorite = await favoriteService.CreateFavorite(favoriteDto, parsedUserId);
            return Ok(createdFavorite);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            var favorites = await favoriteService.GetFavoritesByUserId(parsedUserId);
            return Ok(favorites);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            await favoriteService.DeleteFavorite(id);
            return NoContent();
        }
    }
}