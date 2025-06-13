using SystemBackend.Models.Entities;

namespace SystemBackend.Models.DTO
{
    public class UserDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }

    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string Role { get; set; } = nameof(UserRoleType.Admin);
    }
    public class SuccessfulCreatePendingUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } = nameof(UserRoleType.Admin);
    }

    public class SignInUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class SuccessfulSignInUserDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } = nameof(UserRoleType.Admin);
        public required string AccessToken { get; set; }

    }

    public class PendingUserDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }

    public class VerifyPendingUserDto
    {
        public required Guid Id { get; set; }
    }
}
