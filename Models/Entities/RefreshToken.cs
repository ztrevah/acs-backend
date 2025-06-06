namespace SystemBackend.Models.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public required DateTime ExpiredAt { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
