using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IAuthService
    {
        public bool CheckPassword(string rawPassword, string hashedPassword);
        public bool CheckExistingUsername(string username);
        public User? GetUserByUsername(string username);
        public User? GetUserById(Guid id);
        public User? CreateUser(CreateUserDto user);
        public string HashedPassword(string rawPassword);
        public string GenerateAccessToken(User user);
        public RefreshToken? GenerateRefreshToken(User user);
        public RefreshToken? GetRefreshToken(string refreshToken);
        public RefreshToken? RemoveRefreshToken(string refreshToken);
    }
}
