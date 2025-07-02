using SystemBackend.Data;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class RoomMemberRepository : IRoomMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoomMemberRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public RoomMember? GetById(Guid id)
        {
            return _dbContext.RoomMembers.FirstOrDefault(x => x.Id == id);
        }
        public RoomMember? GetByRoomAndMember(Guid roomId, string civilianId)
        {
            return _dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomId && rm.MemberId == civilianId);
        }
        public RoomMember? Create(RoomMember roomMember)
        {
            if(_dbContext.Rooms.FirstOrDefault(r => r.Id ==  roomMember.RoomId) == null
                || _dbContext.Civilians.FirstOrDefault(c => c.Id == roomMember.MemberId) == null
                || _dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomMember.RoomId && rm.MemberId == roomMember.MemberId) != null)
            {
                return null;
            }

            if ((roomMember.DisabledEndTime == null && roomMember.DisabledStartTime != null)
                || (roomMember.DisabledEndTime != null && roomMember.DisabledStartTime == null))
            {
                return null;
            }

            if (roomMember.StartTime > roomMember.EndTime || roomMember.DisabledStartTime > roomMember.DisabledEndTime
                || roomMember.DisabledStartTime < roomMember.StartTime || roomMember.DisabledEndTime > roomMember.EndTime)
            {
                return null;
            }

            _dbContext.RoomMembers.Add(roomMember);
            _dbContext.SaveChanges();

            return roomMember;
        }
        public RoomMember? Update(Guid id, RoomMember roomMember)
        {
            var exisitingRoomMember = _dbContext.RoomMembers.FirstOrDefault(rm => rm.Id == id);

            if(exisitingRoomMember == null) { return null; }

            if ((roomMember.DisabledEndTime == null && roomMember.DisabledStartTime != null)
                || (roomMember.DisabledEndTime != null && roomMember.DisabledStartTime == null))
            {
                return null;
            }

            if (roomMember.StartTime > roomMember.EndTime || roomMember.DisabledStartTime > roomMember.DisabledEndTime
               || roomMember.DisabledStartTime < roomMember.StartTime || roomMember.DisabledEndTime > roomMember.EndTime)
            {
                return null;
            }

            exisitingRoomMember.StartTime = roomMember.StartTime;
            exisitingRoomMember.EndTime = roomMember.EndTime;
            exisitingRoomMember.DisabledStartTime = roomMember.DisabledStartTime;
            exisitingRoomMember.DisabledEndTime = roomMember.DisabledEndTime;
            _dbContext.SaveChanges();
            return exisitingRoomMember;
        }

        public RoomMember? Remove(Guid roomId, string civilianId)
        {
            var roomMember = _dbContext.RoomMembers.FirstOrDefault(rm => rm.RoomId == roomId && rm.MemberId == civilianId);
            if (roomMember == null)
            {
                return null;
            }

            if (roomMember.EndTime > DateTime.UtcNow)
            {
                roomMember.EndTime = DateTime.UtcNow;
                roomMember.DisabledStartTime = null;
                roomMember.DisabledEndTime = null;
                _dbContext.SaveChanges();
            }

            return roomMember;
        }

        public List<RoomMember> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.RoomId == roomId);

            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId)
                    .OrderBy(rm => rm.Id);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId)
                    .OrderByDescending(rm => rm.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var roomMembers = query.ToList();

            return roomMembers;
        }
        public List<RoomMember> GetByMemberId(string memberId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == memberId);

            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId)
                    .OrderBy(rm => rm.Id);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId)
                    .OrderByDescending(rm => rm.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var roomMembers = query.ToList();

            return roomMembers;
        }
    }
}
