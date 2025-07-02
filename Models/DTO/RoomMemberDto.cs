using SystemBackend.Models.Entities;

namespace SystemBackend.Models.DTO
{
    public class RoomMemberDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public DateTime? DisabledStartTime { get; set; } = null;
        public DateTime? DisabledEndTime { get; set; } = null;
    }
    
    public class AddRoomMemberDto
    {
        public Guid? RoomId { get; set; }
        public required string MemberId { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public DateTime? DisabledStartTime { get; set; } = null;
        public DateTime? DisabledEndTime { get; set; } = null;
    }

    public class UpdateRoomMemberDto
    {
        public Guid? RoomId { get; set; }
        public string? MemberId { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public required DateTime? DisabledStartTime { get; set; }
        public required DateTime? DisabledEndTime { get; set; }
    }
    public class RoomMemberDetailDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
        public required RoomDto Room { get; set; }
        public required CivilianDto Member {  get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public DateTime? DisabledStartTime { get; set; } = null;
        public DateTime? DisabledEndTime { get; set; } = null;
    }

    public class RoomMemberDetailResponselDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public required string MemberId { get; set; }
        public required CivilianDto Member { get; set; }

        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public DateTime? DisabledStartTime { get; set; } = null;
        public DateTime? DisabledEndTime { get; set; } = null;
    }
}
