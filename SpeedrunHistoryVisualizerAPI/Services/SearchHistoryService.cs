using Microsoft.EntityFrameworkCore;
using SpeedrunHistoryVisualizerAPI.Data;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public class SearchHistoryService(AppDbContext context) : ISearchHistoryService
    {
        public async Task<SearchHistory?> CreateSearchHistory(SearchHistoryDto searchHistoryDto, Guid userId)
        {
            // Check if a search history with the same GameId and UserId already exists
            var existingSearchHistory = await context.SearchHistory
                .FirstOrDefaultAsync(sh => sh.GameId == searchHistoryDto.GameId && sh.UserId == userId);

            if (existingSearchHistory != null)
            {
                // If it exists, call UpdateSearchHistoryLastSearched
                return await UpdateSearchHistoryLastSearched(existingSearchHistory.Id);
                
            }
            else
            {
                // Otherwise, create a new search history entity
                var searchHistory = new SearchHistory
                {
                    GameId = searchHistoryDto.GameId,
                    Name = searchHistoryDto.Name,
                    ImageUri = searchHistoryDto.ImageUri,
                    UserId = userId,
                    LastSearched = DateTime.UtcNow
                };

                context.SearchHistory.Add(searchHistory);
                await context.SaveChangesAsync();
                return searchHistory;
            }
        }

        public async Task<List<SearchHistory>> GetSearchHistoryByUserId(Guid userId)
        {
            return await context.SearchHistory
                .Where(sh => sh.UserId == userId)
                .ToListAsync();
        }

        public async Task<SearchHistory?> UpdateSearchHistoryLastSearched(Guid id)
        {
            var searchHistory = await context.SearchHistory.FindAsync(id);
            if (searchHistory != null)
            {
                searchHistory.LastSearched = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            return searchHistory;
        }

        public async Task DeleteSearchHistory(Guid id)
        {
            var searchHistory = await context.SearchHistory.FindAsync(id);
            if (searchHistory != null)
            {
                context.SearchHistory.Remove(searchHistory);
                await context.SaveChangesAsync();
            }
        }
    }
}