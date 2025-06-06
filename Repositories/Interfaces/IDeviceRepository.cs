using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        public Device? GetById(Guid id);
        public Device Create(Device device);
        public Device? Update(Guid id, Device device);
        public List<Device> Get(Guid? cursorId = null, bool next = true, int limit = 20);
        public List<Device> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int limit = 20);
        public Civilian? GetMember(Guid deviceId, string civilianId);
        public RoomMember? AddMember(Guid deviceId, string civilianId);
    }
}
