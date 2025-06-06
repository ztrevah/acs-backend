namespace SystemBackend.Models.Entities
{
    public class RoomMember
    {
        public Guid Id { get; set; }

        public required Guid RoomId { get; set; }
        public Room Room { get; set; }

        public required string MemberId { get; set; }
        public Civilian Member { get; set; }
    }
}
