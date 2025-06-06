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

        public List<Civilian> Get(string? cursorId = null, bool next = true, int limit = 20)
        {
            var civilians = next ? _dbContext.Civilians
                .OrderBy(c => c.Id)
                .Where(c => cursorId == null || c.Id.CompareTo(cursorId) > 0)
                .Take(limit)
                .ToList() 
                :
                _dbContext.Civilians
                .OrderByDescending(c => c.Id)
                .Where(c => cursorId == null || c.Id.CompareTo(cursorId) < 0)
                .Take(limit)
                .ToList();

            return civilians;
        }

        public List<Room> GetAccessibleRooms(string civilianId, Guid? roomCursorId = null, bool next = true, int limit = 20)
        {
            var rooms = next ?  _dbContext.RoomMembers
                .Where(rm => rm.MemberId == civilianId)
                .Include(rm => rm.Room)
                .OrderBy(rm => rm.RoomId)
                .Where(r => (roomCursorId == null || r.RoomId > roomCursorId))
                .Take(limit)
                .Select(rm => rm.Room)
                .ToList() 
                :
                _dbContext.RoomMembers
                .Where(rm => rm.MemberId == civilianId)
                .Include(rm => rm.Room)
                .OrderByDescending(rm => rm.RoomId)
                .Where(r => (roomCursorId == null || r.RoomId < roomCursorId))
                .Take(limit)
                .Select(rm => rm.Room)
                .ToList();

            return rooms;
        }
    }
}
