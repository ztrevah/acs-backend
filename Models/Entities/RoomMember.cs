namespace SystemBackend.Models.Entities
{
    public class RoomMember
    {
        public Guid Id { get; set; }

        public required Guid RoomId { get; set; }
        public Room Room { get; set; }

        public required string MemberId { get; set; }
        public Civilian Member { get; set; }

        public required DateTime StartTime { get; set; } = DateTime.MinValue;
        public required DateTime EndTime { get; set; } = DateTime.MaxValue;

        public DateTime? DisabledStartTime { get; set; } = null;
        public DateTime? DisabledEndTime { get; set; } = null;
    }
}
