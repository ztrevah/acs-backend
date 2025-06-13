namespace SystemBackend.Models.Entities
{
    public enum UserRoleType
    {
        Admin,
        SuperAdmin
    }

    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        
        public required UserRoleType Role {  get; set; } = UserRoleType.Admin;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
