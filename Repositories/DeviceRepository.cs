using Microsoft.EntityFrameworkCore;
using SystemBackend.Data;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;

namespace SystemBackend.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DeviceRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public Device? GetById(Guid id)
        {
            return _dbContext.Devices.FirstOrDefault(d => d.Id == id);
        }
        public Device Create(Device device)
        {
            _dbContext.Devices.Add(device);
            _dbContext.SaveChanges();
            return device;
        }
        public Device? Update(Guid id, Device device)
        {
            var existingDevice = _dbContext.Devices.FirstOrDefault(d => d.Id == id);
            if(existingDevice == null)
            {
                return null;
            }

            existingDevice.RoomId = device.RoomId;
            _dbContext.SaveChanges();
            return existingDevice;
        }
        public List<Device> Get(Guid? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.Devices.AsQueryable();

            if (next) query = query.Where(d => (cursorId == null || d.Id.CompareTo(cursorId) >= 0))
                .OrderBy(d => d.Id);
            else query = query.Where(d => (cursorId == null || d.Id.CompareTo(cursorId) <= 0))
                .OrderBy(d => d.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var devices = query.ToList();

            return devices;
        }
        public List<Device> GetByRoomId(Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            var query = _dbContext.Devices.AsQueryable();
            query = query.Where(d => d.RoomId == roomId);

            if (next) query = query.Where(d => (cursorId == null || d.Id.CompareTo(cursorId) >= 0))
                    .OrderBy(d => d.Id);
            else query = query.Where(d => (cursorId == null || d.Id.CompareTo(cursorId) <= 0))
                    .OrderByDescending(d => d.Id);

            if (limit != null && limit >= 0) query = query.Take((int)limit);

            var devices = query.ToList();

            return devices;
        }

        public Civilian? GetMember(Guid deviceId, string civilianId)
        {
            var device = _dbContext.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (device == null) { return null; }

            var member = _dbContext.RoomMembers
                .Include(rm => rm.Member)
                .FirstOrDefault(rm => rm.RoomId == device.RoomId && rm.MemberId == civilianId);
            if (member == null) { return null; }

            return member.Member;
        }

        public RoomMember? AddMember(Guid deviceId, String civilianId)
        {
            var device = _dbContext.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (device == null) { return null; }

            if (device.RoomId == null) { return null; }

            if(_dbContext.Rooms.FirstOrDefault(r => r.Id == device.RoomId) == null)
            {
                return null;
            }

            if(_dbContext.Civilians.FirstOrDefault(c => c.Id == civilianId) == null) 
            {
                return null;
            }


            var roomMember = new RoomMember
            {
                RoomId = (Guid) device.RoomId,
                MemberId = civilianId,
            };
            _dbContext.RoomMembers.Add(roomMember);
            _dbContext.SaveChanges();

            return roomMember;

        }
    }
}
