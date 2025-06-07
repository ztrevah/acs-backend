using Microsoft.EntityFrameworkCore;
using SystemBackend.Data;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class CivilianRepository : ICivilianRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CivilianRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public Civilian? GetById(String id)
        {
            return _dbContext.Civilians.FirstOrDefault(c => c.Id == id);
        }
        public Civilian Add(Civilian civilian)
        {
            _dbContext.Civilians.Add(civilian);
            _dbContext.SaveChanges();

            return civilian;
        }

        public Civilian? Update(String id, Civilian civilian)
        {
            var existingCivilian = _dbContext.Civilians.FirstOrDefault(c => c.Id == id);
            if(existingCivilian == null)
            {
                return null;
            }

            existingCivilian.Name = civilian.Name;
            existingCivilian.DateOfBirth = civilian.DateOfBirth;
            existingCivilian.Hometown = civilian.Hometown;
            existingCivilian.Email = civilian.Email;
            existingCivilian.PhoneNumber = civilian.PhoneNumber;
            existingCivilian.ImageUrl = civilian.ImageUrl;
            _dbContext.SaveChanges();
            return existingCivilian;
        }

        public List<Civilian> Get(string? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.Civilians.AsQueryable();

            if (next) query = query.OrderBy(c => c.Id)
                .Where(c => cursorId == null || c.Id.CompareTo(cursorId) >= 0);

            else query = query.OrderByDescending(c => c.Id)
                .Where(c => cursorId == null || c.Id.CompareTo(cursorId) <= 0);

            if (limit != null && limit >= 0) query = query.Take((int) limit);

            var civilians = query.ToList();

            return civilians;
        }

        public List<Room> GetAccessibleRooms(string civilianId, Guid? roomCursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == civilianId)
                .Include(rm => rm.Room);

            if (next) query = query.OrderBy(rm => rm.RoomId)
                .Where(r => (roomCursorId == null || r.RoomId >= roomCursorId));
            else query = query.OrderByDescending(rm => rm.RoomId)
                .Where(r => (roomCursorId == null || r.RoomId <= roomCursorId));

            if (limit != null && limit >= 0) query = query.Take((int) limit);

            var rooms = query.Select(rm => rm.Room)
                .ToList();

            return rooms;
        }
    }
}
