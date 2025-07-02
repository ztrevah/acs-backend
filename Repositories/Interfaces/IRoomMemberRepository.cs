using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRoomMemberRepository
    {
        public RoomMember? GetById(Guid id);
        public RoomMember? GetByRoomAndMember(Guid roomId, string civilianId);
        public RoomMember? Create(RoomMember roomMember);
        public RoomMember? Update(Guid id, RoomMember roomMember);
        public RoomMember? Remove(Guid roomId, string civilianId);
        public List<RoomMember> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public List<RoomMember> GetByMemberId(string memberId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
    }
}
