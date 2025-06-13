using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IPendingUserRepository
    {
        public PendingUser? GetById(Guid id);
        public PendingUser? GetByUsername(string username);
        public PendingUser? GetByEmail(string email);
        public PendingUser? Create(PendingUser user);
        public PendingUser? Remove(PendingUser user);
        public List<PendingUser> Get(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
    }
}
