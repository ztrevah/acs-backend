using SystemBackend.Data;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class PendingUserRepository : IPendingUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PendingUserRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public PendingUser? GetById(Guid id)
        {
            return _dbContext.PendingUsers.FirstOrDefault(u =>  u.Id == id);
        }
        public PendingUser? GetByEmail(string email)
        {
            return _dbContext.PendingUsers.FirstOrDefault(u => u.Email == email);
        }
        public PendingUser? GetByUsername(string username)
        {
            return _dbContext.PendingUsers.FirstOrDefault(u => u.Username == username);
        }

        public PendingUser? Remove(PendingUser user)
        {
            var removedPendingUser = _dbContext.PendingUsers.FirstOrDefault(u => u.Id == user.Id);
            if (removedPendingUser != null)
            {
                _dbContext.PendingUsers.Remove(removedPendingUser);
                _dbContext.SaveChanges();

                return removedPendingUser;
            }

            return null;
        }

        public PendingUser? Create(PendingUser user)
        {
            var existingPendingUser = _dbContext.PendingUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existingPendingUser == null)
            {
                _dbContext.PendingUsers.Add(user);
                _dbContext.SaveChanges();

                return user;
            }

            return null;
        }

        public List<PendingUser> Get(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.PendingUsers.AsQueryable();

            if (keyword != null) query = query.Where(u => u.Id.ToString().StartsWith(keyword) || u.Email.StartsWith(keyword) || u.Username.StartsWith(keyword));
            if (next) query = query.Where(c => cursorId == null || c.Id >= cursorId)
                .OrderBy(c => c.Id);

            else query = query.Where(c => cursorId == null || c.Id <= cursorId)
                .OrderByDescending(c => c.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var pendingUsers = query.ToList();

            return pendingUsers;
        }
    }
}
