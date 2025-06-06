using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public RefreshToken? Get(string token);
        public RefreshToken Create(RefreshToken refreshToken);
        public RefreshToken? Remove(string token);
    }
}
