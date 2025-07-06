using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRoomMemberRepository
    {
        public RoomMember? GetById(Guid id);
        public RoomMember? GetById(Guid id, Guid roomId, string civilianId);
        public List<RoomMember> GetByRoomAndMember(Guid roomId, string civilianId, Guid? cursorId = null, bool next = true, int? limit = null);
        public AccessStatus GetRightStatus(Guid id);
        public AccessStatus GetAccessStatus(Guid roomId, string civilianId);
        public RoomMember? Create(RoomMember roomMember);
        public RoomMember? Update(Guid id, RoomMember roomMember);
        public RoomMember? Remove(Guid id);
        public int Remove(Guid roomId, string civilianId);
        public List<RoomMember> GetAccessRightsByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public List<RoomMember> GetAccessRightsByMemberId(string memberId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public List<Civilian> GetMembersByRoomId(Guid roomId, string? civilianCursorId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false);
        public List<Room> GetAccessibleRoomsByMemberId(string memberId, Guid? cursorRoomId = null, bool next = true, int? limit = null, string? keyword = null);
    }
}
