using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IRoomService
    {
        public Room? GetRoomById(Guid id);
        public List<Room> GetRooms(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);     
        public Room CreateRoom(AddRoomDto addRoomDto);
        public Room? UpdateRoom(Guid id, UpdateRoomDto updateRoomDto);
        public List<Device> GetDevices(Guid roomId, Guid? deviceCursorId = null, bool next = true, int? limit = null, string? keyword = null, bool? isIn = null);
        public Device? AddDevice(Guid roomId, Guid deviceId);
        public List<Civilian> GetMembers(Guid roomId, string? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false);
        public List<RoomMember> GetMemberRights(Guid roomId, string civilianId);
        public AccessStatus GetRightStatus(Guid id);
        public AccessStatus GetRoomMemberStatus(Guid roomId, string civilianId);
        public RoomMember? GetRoomMember(Guid id);
        public RoomMember? GetRoomMember(Guid id, Guid roomId, string civilianId);
        public RoomMember? AddRoomMember(Guid roomId, string civilianId, AddRoomMemberDto addRoomMemberDto);
        public RoomMember? UpdateRoomMember(Guid id, UpdateRoomMemberDto updatedRoomMember);
        public RoomMember? RemoveRoomMember(Guid id);
        public int RemoveAllMemberRights(Guid roomId, string civilianId);
    }
}
