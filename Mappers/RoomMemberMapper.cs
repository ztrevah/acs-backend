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
                Member = roomMember.Member.FromCivilianToCivilianDto()
            };
        }

        public static RoomMemberDto FromRoomMemberToRoomMemberDto(this RoomMember roomMember)
        {
            return new RoomMemberDto
            {
                Id = roomMember.Id,
                RoomId = roomMember.RoomId,
                MemberId = roomMember.MemberId,
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
            };
        }
    }
}
