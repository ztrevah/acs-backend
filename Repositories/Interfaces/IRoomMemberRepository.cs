using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRoomMemberRepository
    {
        public RoomMember? GetById(Guid id);
        public RoomMember Create(RoomMember roomMemberDto);
        public List<RoomMember> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null);
        public List<RoomMember> GetByMemberId(String memberId, Guid? cursorId = null, bool next = true, int? limit = null);
    }
}
