using Microsoft.EntityFrameworkCore;
using SpeedrunHistoryVisualizerAPI.Data;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public class FavoriteService(AppDbContext context) : IFavoriteService
    {
        public async Task<Favorite> CreateFavorite(FavoriteDto favoriteDto, Guid userId)
        {
            var favorite = new Favorite
            {
                GameId = favoriteDto.GameId,
                Name = favoriteDto.Name,
                ImageUri = favoriteDto.ImageUri,
                UserId = userId,
                DateFavorited = DateTime.UtcNow
            };

            context.Favorites.Add(favorite);
            await context.SaveChangesAsync();
            return favorite;
        }

        public async Task<List<Favorite>> GetFavoritesByUserId(Guid userId)
        {
            return await context.Favorites
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteFavorite(Guid id)
        {
            var favorite = await context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                context.Favorites.Remove(favorite);
                await context.SaveChangesAsync();
            }
        }
    }
}