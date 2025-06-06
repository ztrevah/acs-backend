namespace SystemBackend.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
