namespace SystemBackend.Models.Entities
{
    public class Civilian
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Hometown { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<RoomMember> RoomMembers { get; set; } = [];

        public ICollection<Log> Logs { get; set; } = [];
    }
}
