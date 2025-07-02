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

        public List<Civilian> Get(string? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.Civilians.AsQueryable();

            if(keyword != null) query = query.Where(c => c.Id.StartsWith(keyword) || c.Name.Contains(keyword));

            if (next) query = query.Where(c => cursorId == null || c.Id.CompareTo(cursorId) >= 0)
                .OrderBy(c => c.Id);

            else query = query.Where(c => cursorId == null || c.Id.CompareTo(cursorId) <= 0)
                .OrderByDescending(c => c.Id);

            if (limit != null && limit >= 0) query = query.Take((int) limit);

            var civilians = query.ToList();

            return civilians;
        }

        public List<Room> GetAccessibleRooms(string civilianId, Guid? roomCursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == civilianId)
                .Where(rm => rm.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.EndTime
                    && (rm.DisabledStartTime == null || !(rm.DisabledStartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.DisabledEndTime)))
                .Include(rm => rm.Room);

            if (keyword != null) query = query.Where(rm => rm.RoomId.ToString().StartsWith(keyword) || rm.Room.Name.Contains(keyword));

            if (next) query = query.Where(r => (roomCursorId == null || r.RoomId >= roomCursorId))
                .OrderBy(rm => rm.RoomId);
            else query = query.Where(r => (roomCursorId == null || r.RoomId <= roomCursorId))
                .OrderByDescending(rm => rm.RoomId);

            if (limit != null && limit >= 0) query = query.Take((int) limit);

            var rooms = query.Select(rm => rm.Room)
                .ToList();

            return rooms;
        }
    }
}
