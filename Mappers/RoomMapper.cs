using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class RoomMapper
    {
        public static RoomDto FromRoomToRoomDto(this Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Location = room.Location,
                Description = room.Description
            };
        }

        public static RoomDetailDto FromRoomToRoomDetailDto(this Room room)
        {
            return new RoomDetailDto
            {
                Id = room.Id,
                Name = room.Name,
                Location = room.Location,
                Description = room.Description,
                Members = room.Members
                    .Select(m => m.Member)
                    .Select(m => m.FromCivilianToCivilianDto())
                    .ToList(),
                Devices = room.Devices
                    .Select(d => d.FromDeviceToDeviceDto())
                    .ToList()
            };
        }

        public static Room FromAddRoomDtoToRoom(this AddRoomDto addRoomDto)
        {
            return new Room
            {
                Name = addRoomDto.Name,
                Location = addRoomDto.Location,
                Description = addRoomDto.Description,
            };
        }
    }
}
