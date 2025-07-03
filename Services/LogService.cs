using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IImageService _imageService;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICivilianRepository _civilianRepository;
        public LogService(ILogRepository logRepository, IImageService imageService, IDeviceRepository deviceRepository, ICivilianRepository civilianRepository)
        {
            _logRepository = logRepository;
            _imageService = imageService;
            _deviceRepository = deviceRepository;
            _civilianRepository = civilianRepository;
        }
        public Log? GetLogById(Guid id)
        {
            return _logRepository.GetById(id);
        }

        public Log? CreateLog(AddLogFromDeviceDto addLogFromDeviceDto)
        {
            var device = _deviceRepository.GetById(addLogFromDeviceDto.DeviceId);
            if (device == null)
            {
                return null;
            }
            if(device.RoomId == null)
            {
                return null;
            }

            if(_civilianRepository.GetById(addLogFromDeviceDto.CivilianId) == null)
            {
                return null;
            }

            try
            {
                var logImage = _imageService.UploadImage(addLogFromDeviceDto.Image);
                if(logImage == null)
                {
                    return null;
                }

                var newLog = new Log
                {
                    ImageUrl = $"/api/images/{logImage}",
                    DeviceId = addLogFromDeviceDto.DeviceId,
                    RoomId = (Guid)device.RoomId,
                    CivilianId = addLogFromDeviceDto.CivilianId,
                    In = device.In,
                };
                _logRepository.Create(newLog);
                return newLog;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Log> GetLogs(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null)
        {
            return _logRepository.Get(cursorId, fromTime, toTime, isIn, next, limit);
        }

        public List<Log> GetLogsByCivilian(string civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null)
        {
            return _logRepository.GetByCivilianId(civilianId, cursorId, fromTime, toTime, isIn, next, limit);
        }

        public List<Log> GetLogsByDevice(Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null)
        {
            return _logRepository.GetByDeviceId(deviceId, cursorId, fromTime, toTime, isIn, next, limit);
        }

        public List<Log> GetLogsByRoom(Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null)
        {
            return _logRepository.GetByRoomId(roomId, cursorId, fromTime, toTime, isIn, next, limit);
        }
    }
}
