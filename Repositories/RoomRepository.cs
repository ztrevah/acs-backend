using Microsoft.EntityFrameworkCore;
using System.Linq;
using SystemBackend.Data;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoomRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public Room? GetById(Guid id)
        {
            return _dbContext.Rooms.FirstOrDefault(r => r.Id == id);
        }
        public Room Create(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();
            return room;
        }
        public Room? Update(Guid id, Room room)
        {
            var existingRoom = _dbContext.Rooms.FirstOrDefault(room => room.Id == id);
            if(existingRoom == null)
            {
                return null;
            }

            existingRoom.Location = room.Location;
            existingRoom.Name = room.Name;
            existingRoom.Description = room.Description;
            _dbContext.SaveChanges();
            return room;
        }
        public List<Room> Get(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.Rooms.AsQueryable();
            
            if (keyword != null) query = query.Where(r => r.Id.ToString().StartsWith(keyword) || r.Name.Contains(keyword));

            if (next) query = query.Where(r => (cursorId == null || r.Id >= cursorId))
                    .OrderBy(r => r.Id);
            else query = query.Where(r => (cursorId == null || r.Id <= cursorId))
                    .OrderByDescending(r => r.Id);

            if(limit != null && limit >= 0) query = query.Take((int) limit);

            var rooms = query.ToList();

            return rooms;

        }

        public List<Room> GetAccessibleRoomsByMember(string civilianId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == civilianId)
                .Include(rm => rm.Room);

            if (keyword != null) query = query.Where(rm => rm.RoomId.ToString().StartsWith(keyword) || rm.Room.Name.Contains(keyword));

            if (next) query = query.Where(r => (cursorId == null || r.RoomId >= cursorId))
                .OrderBy(rm => rm.RoomId);
            else query = query.Where(r => (cursorId == null || r.RoomId <= cursorId))
                .OrderByDescending(rm => rm.RoomId);

            if (limit != null && limit >= 0) query.Take((int) limit);

            var rooms = query.Select(rm => rm.Room).ToList();
            
            return rooms;
        }

        public Device? AddDeviceToRoom(Guid roomId, Guid deviceId)
        {
            if(_dbContext.Rooms.FirstOrDefault(r => r.Id == roomId) == null)
            {
                return null;
            }

            var device = _dbContext.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (device == null)
            {
                return null;
            }

            device.RoomId = roomId;
            _dbContext.SaveChanges();

            return device;
        }

        public RoomMember? GetRoomMember(Guid roomId, string civilianId)
        {
            return _dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomId && rm.MemberId == civilianId);
        }

        public List<Civilian> GetRoomMembers(Guid roomId, string? civilianCursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();

            query = query.Where(rm => rm.RoomId == roomId)
                .Include(rm => rm.Member);

            if(keyword != null) query = query.Where(rm => rm.MemberId.StartsWith(keyword) || rm.Member.Name.Contains(keyword));

            if (next) query = query.Where(rm => civilianCursorId == null || rm.MemberId.CompareTo(civilianCursorId) >= 0)
                .OrderBy(rm => rm.MemberId);
            else query = query.Where(rm => civilianCursorId == null || rm.MemberId.CompareTo(civilianCursorId) <= 0)
                .OrderByDescending(rm => rm.MemberId);

            if(limit != null && limit >= 0) query = query.Take((int) limit);

            var members = query.Select(rm => rm.Member)
                .ToList();

            return members;
        }

        public RoomMember? AddMemberToRoom(Guid roomId, string civilianId)
        {
            if(_dbContext.Rooms.FirstOrDefault(r => r.Id == roomId) == null)
            {
                return null;
            }

            if(_dbContext.Civilians.FirstOrDefault(c => c.Id == civilianId) == null)
            {
                return null;
            }

            if(_dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomId && rm.MemberId == civilianId) != null)
            {
                return null;
            }

            var newRoomMember = new RoomMember
            {
                RoomId = roomId,
                MemberId = civilianId
            };
            _dbContext.RoomMembers.Add(newRoomMember);
            _dbContext.SaveChanges();

            return newRoomMember;
        }
        public RoomMember? RemoveRoomMember(Guid roomId, string civilianId)
        {
            var roomMember = _dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomId && rm.MemberId == civilianId);
            if (roomMember == null)
            {
                return null;
            }

            _dbContext.RoomMembers.Remove(roomMember);
            _dbContext.SaveChanges();
            return roomMember;
        }
    }
}
