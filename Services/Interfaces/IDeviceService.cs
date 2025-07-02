using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IDeviceService
    {
        public Device? GetDeviceById(Guid id);
        public Device? AddDevice(AddDeviceDto addDeviceDto);
        public Device? UpdateDevice(Guid id, UpdateDeviceDto updateDeviceDto);
        public List<Device> GetDevices(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public List<Device> GetDevicesByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public RoomMember? CheckMemberAccessRight(Guid deviceId, string civilianId);
        public RoomMember? AddMember(Guid deviceId, string civilianId);
    }
}
