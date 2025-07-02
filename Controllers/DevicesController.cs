using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/devices")]
    [ApiController]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ICivilianService _civilianService;

        public DevicesController(IDeviceService deviceService, ICivilianService civilianService)
        {
            _deviceService = deviceService;
            _civilianService = civilianService;
        }

        [HttpGet("")]
        public IActionResult GetDevices(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var devices = _deviceService.GetDevices(cursorId, next, limit + 1, keyword)
                .Select(d => d.FromDeviceToDeviceDto())
                .ToList();

            var nextId = (devices.Count == limit + 1) ? devices.Last().Id : (Guid?)null;
            if (devices.Count == limit + 1) devices.Remove(devices.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = devices.Count,
                data = devices
            });
        }

        [HttpGet("{deviceId}")]
        [AllowAnonymous]
        public IActionResult GetDeviceDetail([FromRoute] Guid deviceId)
        {
            var device = _deviceService.GetDeviceById(deviceId);
            if (device == null)
            {
                return NotFound(new
                {
                    error = new { message = "Device not found." }
                });
            }

            return Ok(device.FromDeviceToDeviceDto());
        }

        [HttpPost("")]
        public IActionResult AddDevice([FromBody] AddDeviceDto addDeviceDto)
        {
            if(_deviceService.GetDeviceById(addDeviceDto.Id) != null)
            {
                return BadRequest(new
                {
                    error = new { message = "Device has already existed." }
                });
            }

            try
            {
                var newDevice = _deviceService.AddDevice(addDeviceDto);
                if(newDevice == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }
                return CreatedAtAction(
                    nameof(GetDeviceDetail), 
                    new { deviceId = newDevice.Id }, 
                    newDevice.FromDeviceToDeviceDto());
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpPut("{deviceId}")]
        public IActionResult UpdateDevice([FromRoute] Guid deviceId, [FromBody] UpdateDeviceDto updateDeviceDto)
        {
            if (_deviceService.GetDeviceById(deviceId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Device not found." }
                });
            }

            try
            {
                var updateDevice = _deviceService.UpdateDevice(deviceId, updateDeviceDto);
                if (updateDevice == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }
                return CreatedAtAction(nameof(GetDeviceDetail), new { deviceId }, updateDevice.FromDeviceToDeviceDto());
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpGet("{deviceId}/members/{civilianId}")]
        [AllowAnonymous]
        public IActionResult CheckAccessRight([FromRoute] Guid deviceId, [FromRoute] string civilianId)
        {
            var device = _deviceService.GetDeviceById(deviceId);
            if (device == null)
            {
                return NotFound(new
                {
                    error = new { message = "Device has not been added to the system yet." }
                });
            }
            if(device.RoomId == null)
            {
                return BadRequest(new
                {
                    error = new { message = "Device has not been added to a room." }
                });
            }

            var allowedRoomMember = _deviceService.CheckMemberAccessRight(deviceId, civilianId);
            if (allowedRoomMember == null)
            {
                return StatusCode(403, new
                {
                    error = new { message = "This person is not allowed." }
                });
            }
            
            return Ok(allowedRoomMember.FromRoomMemberToDetailResponselDto());
        }
    }
}
