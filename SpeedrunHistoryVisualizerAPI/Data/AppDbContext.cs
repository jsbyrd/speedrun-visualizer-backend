using Microsoft.EntityFrameworkCore;
using SpeedrunHistoryVisualizerAPI.Entities;

namespace SpeedrunHistoryVisualizerAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
    }
}
