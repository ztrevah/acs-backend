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
        public List<Log> Get(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null)
        {
            var query = _dbContext.Logs.AsQueryable();
            query = query.Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime));

            if (next) query = query.OrderBy(l => l.CreatedAt);
            else query = query.OrderByDescending(l => l.CreatedAt);

            if(cursorId != null)
            {
                var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
                if (cursorLog != null)
                {
                    if(next) query = query.Where(l => (l.CreatedAt > cursorLog.CreatedAt) 
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                    else query = query.Where(l => (l.CreatedAt < cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                }

            }

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var logs = query.ToList();

            return logs;
        }
        public List<Log> GetByDeviceId(Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null)
        {
            var query = _dbContext.Logs.AsQueryable();
            query = query.Where(l => l.DeviceId == deviceId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime));

            if (next) query = query.OrderBy(l => l.CreatedAt);
            else query = query.OrderByDescending(l => l.CreatedAt);

            if (cursorId != null)
            {
                var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
                if (cursorLog != null)
                {
                    if (next) query = query.Where(l => (l.CreatedAt > cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                    else query = query.Where(l => (l.CreatedAt < cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                }

            }

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var logs = query.ToList();

            return logs;
        }
        
        public List<Log> GetByRoomId(Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null)
        {
            var query = _dbContext.Logs.AsQueryable();
            query = query.Where(l => l.RoomId == roomId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime));

            if (next) query = query.OrderBy(l => l.CreatedAt);
            else query = query.OrderByDescending(l => l.CreatedAt);

            if (cursorId != null)
            {
                var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
                if (cursorLog != null)
                {
                    if (next) query = query.Where(l => (l.CreatedAt > cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                    else query = query.Where(l => (l.CreatedAt < cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                }

            }

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var logs = query.ToList();

            return logs;
        }
        public List<Log> GetByCivilianId(string civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int? limit = null)
        {
            var query = _dbContext.Logs.AsQueryable();
            query = query.Where(l => l.CivilianId == civilianId)
                .Where(l => (fromTime == null && toTime == null)
                                || (fromTime == null && l.CreatedAt <= toTime)
                                || (toTime == null && l.CreatedAt >= fromTime)
                                || (l.CreatedAt >= fromTime && l.CreatedAt <= toTime));

            if (next) query = query.OrderBy(l => l.CreatedAt);
            else query = query.OrderByDescending(l => l.CreatedAt);

            if (cursorId != null)
            {
                var cursorLog = _dbContext.Logs.FirstOrDefault(l => l.Id == cursorId);
                if (cursorLog != null)
                {
                    if (next) query = query.Where(l => (l.CreatedAt > cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                    else query = query.Where(l => (l.CreatedAt < cursorLog.CreatedAt)
                                                    || (l.CreatedAt == cursorLog.CreatedAt && l.Id >= cursorId));
                }

            }

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var logs = query.ToList();

            return logs;
        }
    }
}
