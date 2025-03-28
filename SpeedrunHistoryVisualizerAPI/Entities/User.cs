﻿namespace SpeedrunHistoryVisualizerAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<SearchHistory> SearchHistory { get; } = new List<SearchHistory>();
        public ICollection<Favorite> Favorites { get; } = new List<Favorite>();
    }
}
