using SystemBackend.Data;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public RefreshToken Create(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
            _dbContext.SaveChanges();

            return refreshToken;
        }

        public RefreshToken? Get(string token)
        {
            return _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == token);
        }

        public RefreshToken? Remove(string token)
        {
            var rt = _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == token);
            if (rt == null)
            {
                return null;
            }

            _dbContext.RefreshTokens.Remove(rt);
            return rt;
        }
    }
}
