using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User? GetByUsername(String username);
        public User? GetById(Guid id);
        public User Create(User user);
        public User? GetByEmail(string email);
    }
}
