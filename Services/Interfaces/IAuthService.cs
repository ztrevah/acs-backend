using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IAuthService
    {
        public User? GetUserByUsername(string username);
        public User? GetUserByEmail(string email);
        public User? GetUserById(Guid id);
        public User? CreateUser(CreateUserDto user);
        public PendingUser? GetPendingUserById(Guid id);
        public PendingUser? GetPendingUserByEmail(string email);
        public PendingUser? GetPendingUserByUsername(string username);
        public List<PendingUser> GetPendingUsers(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public PendingUser? CreatePendingUser(CreateUserDto user);
        public PendingUser? RemovePendingUser(PendingUser pendingUser);

        public User? VerifyPendingUser(PendingUser pendingUser);
        public bool CheckPassword(string rawPassword, string hashedPassword);
        public bool CheckExistingUsername(string username);
        public string HashedPassword(string rawPassword);
        public string GenerateAccessToken(User user);
        public RefreshToken? GenerateRefreshToken(User user);
        public RefreshToken? GetRefreshToken(string refreshToken);
        public RefreshToken? RemoveRefreshToken(string refreshToken);
    }
}
