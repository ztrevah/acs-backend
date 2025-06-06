using Azure.Core;

namespace SystemBackend.Models.DTO
{
    public class LogDto
    {
        public Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string ImageUrl { get; set; }
        public required Guid DeviceId { get; set; }
        public required Guid RoomId { get; set; }
        public required string CivilianId { get; set; }
    }

    public class AddLogFromDeviceDto
    {
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required IFormFile Image { get; set; }
        public required Guid DeviceId { get; set; }
        public required string CivilianId { get; set; }
    }
    public class LogDetailDto
    {
        public Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string ImageUrl { get; set; }
        public required Guid DeviceId { get; set; }
        public DeviceDto Device { get; set; }
        public required Guid RoomId { get; set; }
        public RoomDto Room { get; set; }
        public required string CivilianId { get; set; }
        public CivilianDto Civilian { get; set; }
    }
}
