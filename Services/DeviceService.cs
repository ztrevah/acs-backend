using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICivilianRepository _civilianRepository;
        public DeviceService(IDeviceRepository deviceRepository, IRoomRepository roomRepository, ICivilianRepository civilianRepository)
        {
            _deviceRepository = deviceRepository;
            _roomRepository = roomRepository;
            _civilianRepository = civilianRepository;
        }

        public RoomMember? AddMember(Guid deviceId, String civilianId)
        {
            if(_deviceRepository.GetById(deviceId) == null)
            {
                return null;
            }

            if(_civilianRepository.GetById(civilianId) == null)
            {
                return null;
            }

            return _deviceRepository.AddMember(deviceId, civilianId);
        }

        public Civilian? GetMember(Guid deviceId, string memberId)
        {
            return _deviceRepository.GetMember(deviceId, memberId);
        }

        public Device? GetDeviceById(Guid id)
        {
            return _deviceRepository.GetById(id);
        }

        public List<Device> GetDevices(Guid? cursorId = null, bool next = true, int? limit = null)
        {
            return _deviceRepository.Get(cursorId, next, limit);
        }
        public List<Device> GetDevicesByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            if(_roomRepository.GetById(roomId) == null)
            {
                return [];
            }

            return _deviceRepository.GetByRoomId(roomId, cursorId, next, limit);
        }

        public Device? AddDevice(AddDeviceDto addDeviceDto)
        {
            if(addDeviceDto.RoomId != null)
            {
                var deviceRoom = _roomRepository.GetById((Guid)addDeviceDto.RoomId);
                if (deviceRoom == null)
                {
                    return null;
                }
            }

            if(_deviceRepository.GetById(addDeviceDto.Id) != null)
            {
                return null;
            }

            var newDevice = new Device
            {
                Id = addDeviceDto.Id,
                RoomId = addDeviceDto.RoomId,
            };

            return _deviceRepository.Create(newDevice);
        }

        public Device? UpdateDevice(Guid id, UpdateDeviceDto updateDeviceDto)
        {
            var device = _deviceRepository.GetById(id);
            if (device == null)
            {
                return null;
            }

            device.RoomId = updateDeviceDto.RoomId ?? device.RoomId;
            return _deviceRepository.Update(id, device);
        }
    }
}
