using SystemBackend.Models.Entities;

namespace SystemBackend.Models.DTO
{
    public class RoomMemberDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
    }
    
    public class AddRoomMemberDto
    {
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
    }
    public class RoomMemberDetailDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
        public required RoomDto Room { get; set; }
        public required CivilianDto Member {  get; set; }
    }

    public class RoomMemberDetailResponselDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
        public required CivilianDto Member { get; set; }
    }
}
