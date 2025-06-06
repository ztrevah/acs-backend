using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class LogMapper
    {
        public static LogDto FromLogToLogDto(this Log log)
        {
            return new LogDto
            {
                Id = log.Id,
                CreatedAt = log.CreatedAt,
                ImageUrl = log.ImageUrl,
                DeviceId = log.DeviceId,
                RoomId = log.RoomId,
                CivilianId = log.CivilianId,
            };
        }

        public static LogDetailDto FromLogToLogDetailDto(this Log log)
        {
            return new LogDetailDto
            {
                Id = log.Id,
                CreatedAt = log.CreatedAt,
                ImageUrl = log.ImageUrl,
                DeviceId = log.DeviceId,
                RoomId = log.RoomId,
                CivilianId = log.CivilianId,
                Device = log.Device.FromDeviceToDeviceDto(),
                Room = log.Room.FromRoomToRoomDto(),
                Civilian = log.Civilian.FromCivilianToCivilianDto()
            };
        }
    }
}
