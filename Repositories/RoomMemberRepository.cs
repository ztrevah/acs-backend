using SystemBackend.Data;
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
        public RoomMember Create(RoomMember roomMember)
        {
            _dbContext.RoomMembers.Add(roomMember);
            _dbContext.SaveChanges();

            return roomMember;
        }
        public List<RoomMember> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.RoomId == roomId);
            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var roomMembers = query.ToList();

            return roomMembers;
        }
        public List<RoomMember> GetByMemberId(string memberId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.RoomMembers.AsQueryable();
            query = query.Where(rm => rm.MemberId == memberId);
            if (next) query = query.Where(rm => cursorId == null || rm.Id >= cursorId);
            else query = query.Where(rm => cursorId == null || rm.Id <= cursorId);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var roomMembers = query.ToList();

            return roomMembers;
        }
    }
}
