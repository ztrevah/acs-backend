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
        private readonly IRoomMemberRepository _roomMemberRepository;
        public RoomService(IRoomRepository roomRepository, IDeviceRepository deviceRepository, IRoomMemberRepository roomMemberRepository)
        {
            _roomRepository = roomRepository;
            _deviceRepository = deviceRepository;
            _roomMemberRepository = roomMemberRepository;
        }
        public Room? GetRoomById(Guid id)
        {
            return _roomRepository.GetById(id);
        }

        public List<Room> GetRooms(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            return _roomRepository.Get(cursorId, next, limit, keyword);
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

        public List<Device> GetDevices(Guid roomId, Guid? deviceCursorId = null, bool next = true, int? limit = null, string? keyword = null, bool? isIn = null)
        {
            return _deviceRepository.GetByRoomId(roomId, deviceCursorId, next, limit, keyword, isIn);
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


        public List<Civilian> GetMembers(Guid roomId, string? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false)
        {
            return _roomMemberRepository.GetMembersByRoomId(roomId, cursorId, next, limit, keyword, onlyAllowed);
        }
        public List<RoomMember> GetMemberRights(Guid roomId, string civilianId)
        {
            return _roomMemberRepository.GetByRoomAndMember(roomId, civilianId);
        }
        public RoomMember? AddRoomMember(Guid roomId, string civilianId, AddRoomMemberDto addRoomMemberDto)
        {
            var newRoomMember = new RoomMember
            {
                RoomId = roomId,
                MemberId = civilianId,
                StartTime = addRoomMemberDto.StartTime,
                EndTime = addRoomMemberDto.EndTime,
                DisabledStartTime = addRoomMemberDto.DisabledStartTime,
                DisabledEndTime = addRoomMemberDto.DisabledEndTime,
            };

            return _roomMemberRepository.Create(newRoomMember);
        }
        public RoomMember? UpdateRoomMember(Guid id, UpdateRoomMemberDto updateRoomMemberDto)
        {
            var rm = _roomMemberRepository.GetById(id);
            if (rm == null)
            {
                return null;
            }

            rm.StartTime = updateRoomMemberDto.StartTime;
            rm.EndTime = updateRoomMemberDto.EndTime;
            rm.DisabledStartTime = updateRoomMemberDto.DisabledStartTime;
            rm.DisabledEndTime = updateRoomMemberDto.DisabledEndTime;

            return _roomMemberRepository.Update(rm.Id, rm);
        }
        public RoomMember? RemoveRoomMember(Guid id)
        {
            return _roomMemberRepository.Remove(id);
        }
        public int RemoveAllMemberRights(Guid roomId, string civilianId)
        {
            return _roomMemberRepository.Remove(roomId, civilianId);
        }

        public AccessStatus GetRightStatus(Guid id)
        {
            return _roomMemberRepository.GetRightStatus(id);
        }

        public AccessStatus GetRoomMemberStatus(Guid roomId, string civilianId)
        {
            return _roomMemberRepository.GetAccessStatus(roomId, civilianId);
        }

        public RoomMember? GetRoomMember(Guid id)
        {
            return _roomMemberRepository.GetById(id);
        }

        public RoomMember? GetRoomMember(Guid id,Guid roomId, string civilianId)
        {
            return _roomMemberRepository.GetById(id, roomId, civilianId);
        }
    }
}
