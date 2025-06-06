namespace SystemBackend.Models.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }

        public ICollection<RoomMember> Members { get; set; } = [];

        public ICollection<Device> Devices { get; set; } = [];

        public ICollection<Log> Logs { get; set; } = [];
    }
}
