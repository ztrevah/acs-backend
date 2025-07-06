using Microsoft.EntityFrameworkCore;
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
        public RoomMember? GetById(Guid id, Guid roomId, string civilianId)
        {
            return _dbContext.RoomMembers.FirstOrDefault(x => x.Id == id && x.RoomId == roomId && x.MemberId == civilianId);
        }
        public List<RoomMember> GetByRoomAndMember(Guid roomId, string civilianId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();

            query = query.Where(rm => rm.RoomId == roomId && rm.MemberId == civilianId);

            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId)
                    .OrderBy(rm => rm.Id);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId)
                    .OrderByDescending(rm => rm.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            return query.ToList();
        }

        public AccessStatus GetRightStatus(Guid id)
        {
            var right = _dbContext.RoomMembers.FirstOrDefault(r => r.Id == id);
            if (right == null) return AccessStatus.UNKNOWN;

            if (right.StartTime > DateTime.UtcNow) return AccessStatus.PENDING;

            if (right.EndTime < DateTime.UtcNow) return AccessStatus.EXPIRED;

            if (right.DisabledStartTime <= DateTime.UtcNow && DateTime.UtcNow <= right.DisabledEndTime) return AccessStatus.DISABLED;

            return AccessStatus.ALLOWED;
        }

        public AccessStatus GetAccessStatus(Guid roomId, string civilianId)
        {
            List<RoomMember> accessRights = GetByRoomAndMember(roomId, civilianId);
            AccessStatus accessStatus = AccessStatus.UNKNOWN;

            foreach (var right in accessRights)
            {
                if (GetRightStatus(right.Id) < accessStatus)
                {
                    accessStatus = GetRightStatus(right.Id);
                }
            }

            return accessStatus;
        }

        public RoomMember? Create(RoomMember roomMember)
        {
            if(_dbContext.Rooms.FirstOrDefault(r => r.Id ==  roomMember.RoomId) == null
                || _dbContext.Civilians.FirstOrDefault(c => c.Id == roomMember.MemberId) == null)
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

            if(GetByRoomAndMember(roomMember.RoomId, roomMember.MemberId).Count >= 20) { return null; }

            var conflictedMemberRights = _dbContext.RoomMembers
                .Where(rm => rm.MemberId == roomMember.MemberId && rm.RoomId == roomMember.RoomId)
                .Where(rm => !(rm.EndTime <= roomMember.StartTime || rm.StartTime >= roomMember.EndTime))
                .ToList();

            if(conflictedMemberRights.Count > 0)
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

            var conflictedMemberRights = _dbContext.RoomMembers
                .Where(rm => rm.MemberId == exisitingRoomMember.MemberId && rm.RoomId == exisitingRoomMember.RoomId)
                .Where(rm => !(rm.EndTime <= roomMember.StartTime || rm.StartTime >= roomMember.EndTime))
                .ToList();

            if (conflictedMemberRights.Count > 1)
            {
                Console.WriteLine(conflictedMemberRights.Count);
                return null;
            }

            if(conflictedMemberRights.Count == 1 && conflictedMemberRights.First().Id != id)
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

        public int Remove(Guid roomId, string civilianId)
        {
            var roomMembers = _dbContext.RoomMembers.Where(rm => rm.RoomId == roomId && rm.MemberId == civilianId).ToList();
            _dbContext.RemoveRange(roomMembers);
            _dbContext.SaveChanges();

            return roomMembers.Count;
        }

        public RoomMember? Remove(Guid id)
        {
            var roomMember = _dbContext.RoomMembers.FirstOrDefault(rm => rm.Id == id);
            if(roomMember == null) return null;

            _dbContext.Remove(roomMember);
            _dbContext.SaveChanges();
            return roomMember;
        }

        public List<RoomMember> GetAccessRightsByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
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
        public List<RoomMember> GetAccessRightsByMemberId(string memberId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == memberId)
                .Include(rm => rm.Room);

            if (keyword != null) query = query.Where(rm => rm.RoomId.ToString().StartsWith(keyword) || rm.Room.Name.Contains(keyword));

            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId)
                    .OrderBy(rm => rm.Id);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId)
                    .OrderByDescending(rm => rm.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var roomMembers = query.ToList();

            return roomMembers;
        } 

        public List<Civilian> GetMembersByRoomId(Guid roomId, string? civilianCursorId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false)
        {
            var query = _dbContext.RoomMembers.AsQueryable();

            query = query.Where(rm => rm.RoomId == roomId)
                .Include(rm => rm.Member);

            if (keyword != null) query = query.Where(c => c.Member.Id.StartsWith(keyword) || c.Member.Name.Contains(keyword));

            if (onlyAllowed)
            {
                query = query.Where(rm => rm.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.EndTime
                                        && (rm.DisabledStartTime == null || !(rm.DisabledStartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.DisabledEndTime)));
            }

            if (next) query = query.Where(c => civilianCursorId == null || c.MemberId.CompareTo(civilianCursorId) >= 0)
                .OrderBy(c => c.Id);

            else query = query.Where(c => civilianCursorId == null || c.MemberId.CompareTo(civilianCursorId) <= 0)
                .OrderByDescending(c => c.Id);


            var civilians = query.Select(rm => rm.Member).Distinct();

            if (limit != null && limit >= 0) civilians = civilians.Take((int)limit);

            return civilians.ToList();
        }

        public List<Room> GetAccessibleRoomsByMemberId(string memberId, Guid? cursorRoomId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == memberId)
                .Where(rm => rm.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.EndTime
                            && (rm.DisabledStartTime == null || !(rm.DisabledStartTime <= DateTime.UtcNow && DateTime.UtcNow <= rm.DisabledEndTime)))
                .Include(rm => rm.Room);

            if (keyword != null) query = query.Where(rm => rm.RoomId.ToString().StartsWith(keyword) || rm.Room.Name.Contains(keyword));

            if (next) query = query.Where(r => (cursorRoomId == null || r.RoomId >= cursorRoomId))
                .OrderBy(rm => rm.RoomId);
            else query = query.Where(r => (cursorRoomId == null || r.RoomId <= cursorRoomId))
                .OrderByDescending(rm => rm.RoomId);

            var rooms = query.Select(rm => rm.Room).Distinct();
            if (limit != null && limit >= 0) rooms = rooms.Take((int)limit);

            return rooms.ToList();
        }
    }
}
