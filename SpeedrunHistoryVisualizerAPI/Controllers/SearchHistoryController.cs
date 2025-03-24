using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;
using SpeedrunHistoryVisualizerAPI.Services;
using System.Security.Claims;

namespace SpeedrunHistoryVisualizerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHistoryController(ISearchHistoryService searchHistoryService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSearchHistory([FromBody] SearchHistoryDto searchHistoryDto)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            SearchHistory? createdSearchHistory = await searchHistoryService.CreateSearchHistory(searchHistoryDto, parsedUserId);

            if (createdSearchHistory == null)
            {
                return StatusCode(500, "Failed to create search history.");
            }
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSearchHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            var searchHistory = await searchHistoryService.GetSearchHistoryByUserId(parsedUserId);
            return Ok(searchHistory);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSearchHistoryLastSearched(Guid id)
        {
            var updatedSearchHistory = await searchHistoryService.UpdateSearchHistoryLastSearched(id);
            if (updatedSearchHistory == null)
            {
                return NotFound();
            }
            return Ok(updatedSearchHistory);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchHistory(Guid id)
        {
            await searchHistoryService.DeleteSearchHistory(id);
            return NoContent();
        }
    }
}