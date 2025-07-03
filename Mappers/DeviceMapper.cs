using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceDto FromDeviceToDeviceDto(this Device device)
        {
            return new DeviceDto
            {
                Id = device.Id,
                RoomId = device.RoomId,
                In = device.In,
            };
        }
    }
}
