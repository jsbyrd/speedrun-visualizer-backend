namespace SpeedrunHistoryVisualizerAPI.Entities
{
    public class Favorite
    {
        public Guid Id { get; set; }
        public string GameId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUri { get; set; } = string.Empty;
        public DateTime DateFavorited { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
