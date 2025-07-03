namespace SystemBackend.Models.Entities
{
    public class Device
    {
        public required Guid Id { get; set; }
        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }
        public required bool In { get; set; }

        public ICollection<Log> Logs { get; set; } = [];
    }
}
