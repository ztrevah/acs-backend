using Microsoft.EntityFrameworkCore;
using SystemBackend.Data;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LogRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public Log? GetById(Guid id)
        {
            return _dbContext.Logs.FirstOrDefault(l => l.Id == id);
        }
        public Log? Create(Log log)
        {
            _dbContext.Logs.Add(log);
            _dbContext.SaveChanges();
            return log;
        }
        public List<Log> Get(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20)
        {
            List<Log> logs = [];
            if(cursorId == null)
            {
                logs = next ? _dbContext.Logs
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderBy(l => l.CreatedAt)
                    .Take(limit)
                    .ToList()
                    :
                    _dbContext.Logs
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(limit)
                    .ToList();

                return logs;
            }
            var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
            if(cursorLog == null)
            {
                return [];
            }

            logs = next ? _dbContext.Logs
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt > cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderBy(l => l.CreatedAt)
                .Take(limit)
                .ToList()
                :
                _dbContext.Logs
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt < cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderByDescending(l => l.CreatedAt)
                .Take(limit)
                .ToList();

            return logs;
        }
        public List<Log> GetByDeviceId(Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20)
        {
            List<Log> logs = [];
            if (cursorId == null)
            {
                logs = next ? _dbContext.Logs
                    .Where(l => l.DeviceId == deviceId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderBy(l => l.CreatedAt)
                    .Take(limit)
                    .ToList()
                    :
                    _dbContext.Logs
                    .Where(l => l.DeviceId == deviceId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(limit)
                    .ToList();

                return logs;
            }
            var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
            if (cursorLog == null)
            {
                return [];
            }

            logs = next ? _dbContext.Logs
                .Where(l => l.DeviceId == deviceId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt > cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderBy(l => l.CreatedAt)
                .Take(limit)
                .ToList()
                :
                _dbContext.Logs
                .Where(l => l.DeviceId == deviceId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt < cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderByDescending(l => l.CreatedAt)
                .Take(limit)
                .ToList();

            return logs;
        }
        
        public List<Log> GetByRoomId(Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20)
        {
            List<Log> logs = [];
            if (cursorId == null)
            {
                logs = next ? _dbContext.Logs
                    .Where(l => l.RoomId == roomId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderBy(l => l.CreatedAt)
                    .Take(limit)
                    .ToList()
                    :
                    _dbContext.Logs
                    .Where(l => l.RoomId == roomId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(limit)
                    .ToList();

                return logs;
            }
            var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
            if (cursorLog == null)
            {
                return [];
            }

            logs = next ? _dbContext.Logs
                    .Where(l => l.RoomId == roomId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .Where(l => l.CreatedAt > cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                    .OrderBy(l => l.CreatedAt)
                    .Take(limit)
                    .ToList()
                    :
                     _dbContext.Logs
                    .Where(l => l.RoomId == roomId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .Where(l => l.CreatedAt < cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(limit)
                    .ToList();

            return logs;
        }
        public List<Log> GetByCivilianId(String civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20)
        {
            List<Log> logs = [];
            if (cursorId == null)
            {
                logs = next ? _dbContext.Logs
                    .Where(l => l.CivilianId == civilianId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderBy(l => l.CreatedAt)
                    .Take(limit)
                    .ToList()
                    :
                    _dbContext.Logs
                    .Where(l => l.CivilianId == civilianId)
                    .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(limit)
                    .ToList();

                return logs;
            }
            var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
            if (cursorLog == null)
            {
                return [];
            }

            logs = next ? _dbContext.Logs
                .Where(l => l.CivilianId == civilianId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt > cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderBy(l => l.CreatedAt)
                .Take(limit)
                .ToList()
                :
                _dbContext.Logs
                .Where(l => l.CivilianId == civilianId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime))
                .Where(l => l.CreatedAt < cursorLog.CreatedAt || (l.CreatedAt == cursorLog.CreatedAt && l.Id > cursorId))
                .OrderByDescending(l => l.CreatedAt)
                .Take(limit)
                .ToList();

            return logs;
        }
    }
}
