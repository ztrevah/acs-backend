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
        private readonly IRoomMemberRepository _roomMemberRepository;
        public DeviceService(IDeviceRepository deviceRepository, IRoomRepository roomRepository, ICivilianRepository civilianRepository, IRoomMemberRepository roomMemberRepository)
        {
            _deviceRepository = deviceRepository;
            _roomRepository = roomRepository;
            _civilianRepository = civilianRepository;
            _roomMemberRepository = roomMemberRepository;
        }

        public RoomMember? AddMember(Guid deviceId, string civilianId)
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

        public AccessStatus CheckMemberAccessRight(Guid deviceId, string civilianId)
        {
            var device = _deviceRepository.GetById(deviceId);

            if (device == null || device.RoomId == null)
            {
                return AccessStatus.UNKNOWN;
            }

            return _roomMemberRepository.GetAccessStatus((Guid) device.RoomId, civilianId);
        }

        public Device? GetDeviceById(Guid id)
        {
            return _deviceRepository.GetById(id);
        }

        public List<Device> GetDevices(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool? isIn = null)
        {
            return _deviceRepository.Get(cursorId, next, limit, keyword, isIn);
        }
        public List<Device> GetDevicesByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool? isIn = null)
        {
            if(_roomRepository.GetById(roomId) == null)
            {
                return [];
            }

            return _deviceRepository.GetByRoomId(roomId, cursorId, next, limit, keyword, isIn);
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
                In = addDeviceDto.In,
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

            device.RoomId = updateDeviceDto.RoomId;
            device.In = updateDeviceDto.In;
            return _deviceRepository.Update(id, device);
        }
    }
}
