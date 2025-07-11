﻿using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface ILogRepository
    {
        public Log? GetById(Guid id);
        public Log? Create(Log log);
        public List<Log> Get(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null);
        public List<Log> GetByDeviceId(Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null);
        public List<Log> GetByRoomId(Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null);
        public List<Log> GetByCivilianId(String civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool? isIn = null, bool next = false, int? limit = null);
    }
}
