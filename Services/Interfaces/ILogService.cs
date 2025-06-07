using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface ILogService
    {
        public Log? GetLogById(Guid id);
        public Log? CreateLog(AddLogFromDeviceDto addLogFromDeviceDto);
        public List<Log> GetLogs(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null);
        public List<Log> GetLogsByDevice(Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null);
        public List<Log> GetLogsByRoom(Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null);
        public List<Log> GetLogsByCivilian(string civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null);
    }
}
