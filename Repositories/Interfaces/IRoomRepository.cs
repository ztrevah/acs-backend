using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        public Room? GetById(Guid id);
        public Room Create(Room room);
        public Room? Update(Guid id, Room room);
        public List<Room> Get(Guid? cursorId = null, bool next = true, int? limit = null);
        public List<Room> GetAccessibleRoomsByMember(String civilianId, Guid? cursorId = null, bool next = true, int? limit = null);
        public Device? AddDeviceToRoom(Guid roomId, Guid deviceId);
        public RoomMember? GetRoomMember(Guid roomId, string civilianId);
        public List<Civilian> GetRoomMembers(Guid roomId, string? civilianCursorId = null, bool next = true, int? limit = null);
        public RoomMember? AddMemberToRoom(Guid roomId, string civilianId);
        public RoomMember? RemoveRoomMember(Guid roomId, string civilianId);
    }
}
