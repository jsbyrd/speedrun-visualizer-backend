using SpeedrunHistoryVisualizerAPI.Entities;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string userId);

    }
}
