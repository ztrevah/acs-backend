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
        public List<RoomMember> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int limit = 20)
        {
            var roomMembers = next ? _dbContext.RoomMembers
                .Where(rm => rm.RoomId == roomId)
                .Where(rm => cursorId == null || rm.Id > cursorId)
                .OrderBy(rm => rm.Id)
                .Take(limit)
                .ToList()
                :
                _dbContext.RoomMembers
                .Where(rm => rm.RoomId == roomId)
                .Where(rm => cursorId == null || rm.Id < cursorId)
                .OrderByDescending(rm => rm.Id)
                .Take(limit)
                .ToList();

            return roomMembers;
        }
        public List<RoomMember> GetByMemberId(string memberId, Guid? cursorId = null, bool next = true, int limit = 20)
        {
            var roomMembers = next ? _dbContext.RoomMembers
                .Where(rm => rm.MemberId == memberId)
                .Where(rm => cursorId == null || rm.Id > cursorId)
                .OrderBy(rm => rm.Id)
                .Take(limit)
                .ToList()
                :
                _dbContext.RoomMembers
                .Where(rm => rm.MemberId == memberId)
                .Where(rm => cursorId == null || rm.Id < cursorId)
                .OrderByDescending(rm => rm.Id)
                .Take(limit)
                .ToList();

            return roomMembers;
        }
    }
}
