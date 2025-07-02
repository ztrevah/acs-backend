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

        public List<Device> GetDevices(Guid roomId, Guid? deviceCursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            return _deviceRepository.GetByRoomId(roomId, deviceCursorId, next, limit, keyword);
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

        public List<RoomMember> GetMembers(Guid roomId, Guid? roomMemberId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false)
        {
            return _roomRepository.GetRoomMembers(roomId, roomMemberId, next, limit, keyword, onlyAllowed);
        }
        public RoomMember? AddRoomMember(Guid roomId, AddRoomMemberDto addRoomMemberDto)
        {
            //if (addRoomMemberDto.StartTime > addRoomMemberDto.EndTime || addRoomMemberDto.DisabledStartTime > addRoomMemberDto.DisabledEndTime
            //    || addRoomMemberDto.DisabledStartTime < addRoomMemberDto.StartTime || addRoomMemberDto.DisabledEndTime > addRoomMemberDto.EndTime)
            //{
            //    return null;
            //}

            var newRoomMember = new RoomMember
            {
                RoomId = roomId,
                MemberId = addRoomMemberDto.MemberId,
                StartTime = addRoomMemberDto.StartTime,
                EndTime = addRoomMemberDto.EndTime,
                DisabledStartTime = addRoomMemberDto.DisabledStartTime,
                DisabledEndTime = addRoomMemberDto.DisabledEndTime,
            };

            return _roomMemberRepository.Create(newRoomMember);
        }
        public RoomMember? UpdateRoomMember(Guid roomId, string civilianId, UpdateRoomMemberDto updateRoomMemberDto)
        {
            var rm = _roomMemberRepository.GetByRoomAndMember(roomId, civilianId);
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
        public RoomMember? RemoveMember(Guid roomId, string civilianId)
        {
            return _roomMemberRepository.Remove(roomId, civilianId);
        }
    }
}
