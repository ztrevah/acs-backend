using SystemBackend.Data;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public User? GetByUsername(String username)
        {
            return _dbContext.Users.FirstOrDefault(a => a.Username == username);
        }
        public User? GetById(Guid id)
        {
            return _dbContext.Users.FirstOrDefault(a => a.Id == id);
        }
        public User Create(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
    }
}
