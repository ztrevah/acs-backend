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
        public List<RoomMember> GetMembers(Guid roomId, Guid? roomMemberId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false);
        public RoomMember? GetMember(Guid roomId, string civilianId);
        public RoomMember? AddRoomMember(Guid roomId, AddRoomMemberDto addRoomMemberDto);
        public RoomMember? UpdateRoomMember(Guid roomId, string civilianId, UpdateRoomMemberDto updatedRoomMember);
        public RoomMember? RemoveMember(Guid roomId, string civilianId);
    }
}
