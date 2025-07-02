using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBackend.Data;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/logs")]
    [ApiController]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly ICivilianService _civilianService;
        private readonly IDeviceService _deviceService;
        private readonly IRoomService _roomService;
        public LogsController(ILogService logService, ICivilianService civilianService, IDeviceService deviceService, IRoomService roomService)
        {
            _logService = logService;
            _civilianService = civilianService;
            _deviceService = deviceService;
            _roomService = roomService;
        }

        [HttpGet("{logId}")]
        public IActionResult GetLogById(Guid logId)
        {
            var log = _logService.GetLogById(logId);
            if (log == null)
            {
                return NotFound(new
                {
                    error = new { message = "Log not found." }
                });
            }

            return Ok(log.FromLogToLogDto());
        }

        [HttpGet("")]
        public IActionResult GetLogs(Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20, string? keyword = null)
        {
            if(limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }
            var logs = _logService.GetLogs(cursorId, fromTime, toTime, next, limit + 1);

            var nextId = (logs.Count == limit + 1) ? logs.Last().Id : (Guid?)null;
            if (logs.Count == limit + 1) logs.Remove(logs.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = logs.Count,
                data = logs
            });
        }

        [HttpGet("civilian/{civilianId}")]
        public IActionResult GetLogsByCivilian([FromRoute] string civilianId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            if (_civilianService.GetCivilianById(civilianId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }

            var logs = _logService.GetLogsByCivilian(civilianId, cursorId, fromTime, toTime, next, limit + 1);

            var nextId = (logs.Count == limit + 1) ? logs.Last().Id : (Guid?)null;
            if (logs.Count == limit + 1) logs.Remove(logs.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = logs.Count,
                data = logs
            });
        }

        [HttpGet("room/{roomId}")]
        public IActionResult GetLogsByRoom([FromRoute] Guid roomId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            if (_roomService.GetRoomById(roomId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var logs = _logService.GetLogsByRoom(roomId, cursorId, fromTime, toTime, next, limit + 1);

            var nextId = (logs.Count == limit + 1) ? logs.Last().Id : (Guid?)null;
            if (logs.Count == limit + 1) logs.Remove(logs.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = logs.Count,
                data = logs
            });
        }

        [HttpGet("device/{deviceId}")]
        public IActionResult GetLogsByDevice([FromRoute] Guid deviceId, Guid? cursorId = null, DateTime? fromTime = null, DateTime? toTime = null, bool next = false, int limit = 20, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            if (_deviceService.GetDeviceById(deviceId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Device not found." }
                });
            }

            var logs = _logService.GetLogsByDevice(deviceId, cursorId, fromTime, toTime, next, limit + 1);

            var nextId = (logs.Count == limit + 1) ? logs.Last().Id : (Guid?)null;
            if (logs.Count == limit + 1) logs.Remove(logs.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = logs.Count,
                data = logs
            });
        }

        [HttpPost("")]
        [AllowAnonymous]
        public IActionResult AddLog([FromForm] AddLogFromDeviceDto addLogDto)
        {
            var device = _deviceService.GetDeviceById(addLogDto.DeviceId);
            if (device == null)
            {
                return NotFound(new
                {
                    error = new { message = "Device not found." }
                });
            }

            if (device.RoomId == null)
            {
                return BadRequest(new
                {
                    error = new { message = "Device has not been added to a room." }
                });
            }

            var roomMember = _deviceService.CheckMemberAccessRight(addLogDto.DeviceId, addLogDto.CivilianId);
            if (roomMember == null)
            {
                return StatusCode(403, new 
                {
                    error = new { message = "This civilian is not a member of this room." }
                });
            }

            try
            {
                var newLog = _logService.CreateLog(addLogDto);
                if(newLog == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });

                }

                return CreatedAtAction(nameof(GetLogById), new { logId = newLog.Id }, newLog.FromLogToLogDto());
            }
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }
    }
}
