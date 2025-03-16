using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpeedrunHistoryVisualizerAPI.Data;
using SpeedrunHistoryVisualizerAPI.Entities;
using SpeedrunHistoryVisualizerAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpeedrunHistoryVisualizerAPI.Services
{
    public class AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        public async Task<bool> LoginAsync(UserDto request)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null) return false;
            if (user.Username != request.Username) return false;
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed) return false;

            await GenerateAndSaveTokens(user);
            return true;
        }

        public async Task<bool> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username)) return false;

            User user = new User();
            string hashedPassword = new PasswordHasher<User>()
               .HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await GenerateAndSaveTokens(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefreshTokensAsync()
        {
            var request = httpContextAccessor.HttpContext?.Request;
            var response = httpContextAccessor.HttpContext?.Response;

            if (request == null || response == null) return false;

            if (!request.Cookies.TryGetValue("Refresh", out string? refreshToken) || refreshToken == null)
                return false;

            var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user is null) return false;

            await GenerateAndSaveTokens(user);
            return true;
        }

        public async Task LogoutAsync(string userId)
        {
            var response = httpContextAccessor.HttpContext?.Response;
            if (response == null) return;

            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user is null) return;

            response.Cookies.Delete("Authentication");
            response.Cookies.Delete("Refresh");

            user.RefreshToken = null;
            await context.SaveChangesAsync();
        }

        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string CreateRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task GenerateAndSaveTokens(User user)
        {
            var accessToken = CreateAccessToken(user);
            var refreshToken = CreateRefreshToken(user);

            DateTime accessTokenExpiryDate = DateTime.UtcNow.AddMinutes(1);
            DateTime refreshTokenExpiryDate = DateTime.UtcNow.AddDays(1);

            var response = httpContextAccessor.HttpContext?.Response;
            if (response == null) return;

            response.Cookies.Append("Authentication", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = accessTokenExpiryDate,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            response.Cookies.Append("Refresh", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiryDate,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryDate;
            await context.SaveChangesAsync();
        }
    }
}
