using SystemBackend.Models.Entities;

namespace SystemBackend.Models.DTO
{
    public class AddRoomDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Location { get; set; }
    }

    public class UpdateRoomDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Location { get; set; }
    }

    public class RoomDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Location { get; set; }
    }

    public class RoomDetailDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Location { get; set; }
        public ICollection<CivilianDto> Members { get; set; } = [];
        public ICollection<DeviceDto> Devices { get; set; } = [];
    }
}
