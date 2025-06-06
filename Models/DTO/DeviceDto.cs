namespace SystemBackend.Models.DTO
{
    public class DeviceDto
    {
        public required Guid Id { get; set; }
        public Guid? RoomId { get; set; }
    }

    public class AddDeviceDto
    {
        public required Guid Id { get; set; }
        public Guid? RoomId { get; set; }
    }

    public class UpdateDeviceDto
    {
        public Guid? Id { get; set; }
        public Guid? RoomId { get; set; }
    }

    public class AddDeviceToRoomDto
    {
        public required Guid deviceId { get; set; }
    }
}
