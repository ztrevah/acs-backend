namespace SystemBackend.Models.DTO
{
    public class UserDto
    {
        public required string Username { get; set; }
    }

    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public class SuccessfulCreateUserDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
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
        public required string AccessToken { get; set; }
    }
}
