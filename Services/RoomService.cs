using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IDeviceRepository _deviceRepository;
        public RoomService(IRoomRepository roomRepository, IDeviceRepository deviceRepository)
        {
            _roomRepository = roomRepository;
            _deviceRepository = deviceRepository;
        }
        public Room? GetRoomById(Guid id)
        {
            return _roomRepository.GetById(id);
        }

        public List<Room> GetRooms(Guid? cursorId = null, bool next = true, int? limit = null)
        {
            return _roomRepository.Get(cursorId, next, limit);
        }
        public Room CreateRoom(AddRoomDto addRoomDto)
        {
            return _roomRepository.Create(addRoomDto.FromAddRoomDtoToRoom());
        }
        public Room? UpdateRoom(Guid id, UpdateRoomDto updateRoomDto)
        {
            var room = _roomRepository.GetById(id);
            if(room == null)
            {
                return null;
            }

            room.Name = updateRoomDto.Name ?? room.Name;
            room.Location = updateRoomDto.Location ?? room.Location;
            room.Description = updateRoomDto.Description ?? room.Description;
            return _roomRepository.Update(id, room);
        }

        public List<Device> GetDevices(Guid roomId, Guid? deviceCursorId = null, bool next = true, int? limit = null)
        {
            return _deviceRepository.GetByRoomId(roomId, deviceCursorId, next, limit);
        }
        public Device? AddDevice(Guid roomId, Guid deviceId)
        {
            if(_roomRepository.GetById(roomId) == null)
            {
                return null;
            }
            if(_deviceRepository.GetById(deviceId) == null)
            {
                return null;
            }

            return _roomRepository.AddDeviceToRoom(roomId, deviceId);
        }

        public RoomMember? GetMember(Guid roomId, string civilianId)
        {
            return _roomRepository.GetRoomMember(roomId, civilianId);
        }

        public List<Civilian> GetMembers(Guid roomId, string? civilianCursorId = null, bool next = true, int? limit = null)
        {
            return _roomRepository.GetRoomMembers(roomId, civilianCursorId, next, limit);
        }
        public RoomMember? AddRoomMember(Guid roomId, string civilianId)
        {
            return _roomRepository.AddMemberToRoom(roomId, civilianId);
        }

        public RoomMember? RemoveMember(Guid roomId, string civilianId)
        {
            return _roomRepository.RemoveRoomMember(roomId, civilianId);
        }

        
    }
}
