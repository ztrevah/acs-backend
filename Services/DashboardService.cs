using SystemBackend.Models;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogRepository _logRepository;
        public DashboardService(IRoomRepository roomRepository, IDeviceRepository deviceRepository, ILogRepository logRepository)
        {
            _roomRepository = roomRepository;
            _deviceRepository = deviceRepository;
            _logRepository = logRepository;
        }
    
        public DashboardInformation GetDashboardInformation()
        {
            return new DashboardInformation
            {
                TotalRooms = _roomRepository.Get().Count,
                TotalDevices = _deviceRepository.Get().Count,
                TotalDailyLogs = _logRepository.Get(null, DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1)).Count
            };
        }
    }
}
