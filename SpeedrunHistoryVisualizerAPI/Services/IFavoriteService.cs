using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public interface IFavoriteService
    {
        Task<Favorite> CreateFavorite(FavoriteDto favoriteDto, Guid userId);
        Task<List<Favorite>> GetFavoritesByUserId(Guid userId);
        Task DeleteFavorite(Guid id);
    }
}