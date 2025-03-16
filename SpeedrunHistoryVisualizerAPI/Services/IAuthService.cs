using SpeedrunHistoryVisualizerAPI.Models;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserDto request);
        Task<bool> LoginAsync(UserDto request);
        Task<bool> RefreshTokensAsync();
        Task LogoutAsync(string userId);

    }
}
