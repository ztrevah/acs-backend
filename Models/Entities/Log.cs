using Azure.Core;

namespace SystemBackend.Models.Entities
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required String ImageUrl { get; set; }
        public required Guid DeviceId { get; set; }
        public Device Device { get; set; }

        public required Guid RoomId { get; set; }
        public Room Room { get; set; }
        public required String CivilianId { get; set; }
        public Civilian Civilian { get; set; }

    }
}
