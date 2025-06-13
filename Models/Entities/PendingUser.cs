namespace SystemBackend.Models.Entities
{
    public class PendingUser
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public UserRoleType Role { get; set; } = UserRoleType.Admin;
    }
}
