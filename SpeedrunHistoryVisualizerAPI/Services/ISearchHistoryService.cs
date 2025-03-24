using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public interface ISearchHistoryService
    {
        Task<SearchHistory?> CreateSearchHistory(SearchHistoryDto searchHistoryDto, Guid userId);
        Task<List<SearchHistory>> GetSearchHistoryByUserId(Guid userId);
        Task<SearchHistory?> UpdateSearchHistoryLastSearched(Guid id);
        Task DeleteSearchHistory(Guid id);
    }
}