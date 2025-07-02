using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        public Room? GetById(Guid id);
        public Room Create(Room room);
        public Room? Update(Guid id, Room room);
        public List<Room> Get(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public List<Room> GetAccessibleRoomsByMember(string civilianId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public Device? AddDeviceToRoom(Guid roomId, Guid deviceId);
        public RoomMember? GetRoomMember(Guid roomId, string civilianId);
        public List<RoomMember> GetRoomMembers(Guid roomId, Guid? roomMemberId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false);
        public RoomMember? AddMemberToRoom(RoomMember roomMember);
        public RoomMember? RemoveRoomMember(Guid roomId, string civilianId);
    }
}
