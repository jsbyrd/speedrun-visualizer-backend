using Microsoft.EntityFrameworkCore;
using SpeedrunHistoryVisualizerAPI.Data;
using SpeedrunHistoryVisualizerAPI.Entities;
using System.Security.Claims;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<User?> GetUserById(string userId)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return user;
        }

    }
}
