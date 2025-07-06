using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class RoomMemberMapper
    {
        public static RoomMemberDetailResponselDto FromRoomMemberToDetailResponselDto(this RoomMember roomMember)
        {
            return new RoomMemberDetailResponselDto
            {
                Id = roomMember.Id,
                RoomId = roomMember.RoomId,
                MemberId = roomMember.MemberId,
                Member = roomMember.Member.FromCivilianToCivilianDto(),
                StartTime = roomMember.StartTime,
                EndTime = roomMember.EndTime,
                DisabledStartTime = roomMember.DisabledStartTime,
                DisabledEndTime = roomMember.DisabledEndTime,
            };
        }

        public static RoomMemberDto FromRoomMemberToRoomMemberDto(this RoomMember roomMember)
        {
            return new RoomMemberDto
            {
                Id = roomMember.Id,
                RoomId = roomMember.RoomId,
                MemberId = roomMember.MemberId,
                StartTime = roomMember.StartTime,
                EndTime = roomMember.EndTime,
                DisabledStartTime = roomMember.DisabledStartTime,
                DisabledEndTime = roomMember.DisabledEndTime,
            };
        }

        public static RoomMemberDetailDto FromRoomMemberToRoomMemberDetailDto(this RoomMember roomMember)
        {
            return new RoomMemberDetailDto
            {
                Id = roomMember.Id,
                RoomId = roomMember.RoomId,
                MemberId = roomMember.MemberId,
                Room = roomMember.Room.FromRoomToRoomDto(),
                Member = roomMember.Member.FromCivilianToCivilianDto(),
                StartTime = roomMember.StartTime,
                EndTime = roomMember.EndTime,
                DisabledStartTime = roomMember.DisabledStartTime,
                DisabledEndTime = roomMember.DisabledEndTime,
            };
        }

        public static string FromAccessStatusToString(this AccessStatus status)
        {
            switch (status)
            {
                case AccessStatus.DISABLED:
                    return "Disabled";
                case AccessStatus.ALLOWED:
                    return "Allowed";
                case AccessStatus.PENDING:
                    return "Pending";
                case AccessStatus.EXPIRED:
                    return "Expired";
                default:
                    return "Unknown";
            }
        }
    }
}
