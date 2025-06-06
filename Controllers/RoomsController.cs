using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ICivilianService _civilianService;
        private readonly IDeviceService _deviceService;
        public RoomsController(IRoomService roomService, ICivilianService civilianService, IDeviceService deviceService)
        {
            _roomService = roomService;
            _civilianService = civilianService;
            _deviceService = deviceService;
        }

        [HttpGet("")]
        public IActionResult GetAllRooms(Guid? cursorId = null, bool next = true, int limit = 20)
        {
            var rooms = _roomService.GetRooms(cursorId, next, limit)
                .Select(r => r.FromRoomToRoomDto())
                .ToList();

            return Ok(new
            {
                cursorId = rooms.Count == 0 ? (Guid?)null : rooms.Last().Id,
                count = rooms.Count,
                data = rooms
            });
        }

        [HttpGet("{roomId}")]
        public IActionResult GetRoomDetail([FromRoute] Guid roomId)
        {
            var room = _roomService.GetRoomById(roomId);

            if (room == null)
            {   
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            return Ok(room.FromRoomToRoomDto());
        }

        [HttpPost("")]
        public IActionResult CreateRoom([FromBody] AddRoomDto addRoomDto)
        {
            try
            {
                var newRoom = _roomService.CreateRoom(addRoomDto);
                return CreatedAtAction(
                    nameof(GetRoomDetail),
                    new { roomId = newRoom.Id },
                    newRoom.FromRoomToRoomDto()
                );
            }
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpPut("{roomId}")]
        public IActionResult UpdateRoom([FromRoute] Guid roomId, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            try
            {
                var updatedRoom = _roomService.UpdateRoom(roomId, updateRoomDto);
                if(updatedRoom == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                return Ok(updatedRoom.FromRoomToRoomDto());

            }
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }

        }

        [HttpGet("{roomId}/members")]
        public IActionResult GetRoomMembers([FromRoute] Guid roomId, string? cursorId = null, bool next = true, int limit = 20)
        {
            var room = _roomService.GetRoomById(roomId);
            if(room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var members = _roomService.GetMembers(roomId, cursorId, next, limit)
                .Select(m => m.FromCivilianToCivilianDto())
                .ToList();
            return Ok(new
            {
                cursorId = members.Count == 0 ? null : members.Last().Id,
                count = members.Count,
                data = members
            });
        }

        [HttpPost("{roomId}/members")]
        public IActionResult AddNewMemberToRoom([FromRoute] Guid roomId, [FromBody] AddCivilianToRoomDto addCivilianToRoomDto)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var civilian = _civilianService.GetCivilianById(addCivilianToRoomDto.civilianId);
            if (civilian == null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }

            try
            {
                var newRoomMember = _roomService.AddRoomMember(roomId, addCivilianToRoomDto.civilianId);
                if(newRoomMember == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                var locationUri = $"{Request.Host}/{Request.Path}/{civilian.Id}";
                return Created(locationUri, newRoomMember.FromRoomMemberToRoomMemberDto());
            }
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpDelete("{roomId}/members/{civilianId}")]
        public IActionResult RemoveMemberFromRoom([FromRoute] Guid roomId, [FromRoute] string civilianId)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var civilian = _civilianService.GetCivilianById(civilianId);
            if (civilian == null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }

            try
            {
                var member = _roomService.RemoveMember(roomId, civilianId);
                if (member == null)
                {
                    return NotFound(new
                    {
                        error = new { message = "This civilian has not been a member of this room." }
                    });
                }

                return Ok(member);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpGet("{roomId}/devices")]
        public IActionResult GetRoomDevices([FromRoute] Guid roomId, Guid? cursorId = null, bool next = true, int limit = 20)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var devices = _roomService.GetDevices(roomId, cursorId, next, limit)
                .Select(d => d.FromDeviceToDeviceDto())
                .ToList();
            return Ok(new
            {
                cursorId = devices.Count == 0 ? (Guid?)null : devices.Last().Id,
                count = devices.Count,
                data = devices
            });
        }

        [HttpPost("{roomId}/devices")]
        public IActionResult AddNewDevice([FromRoute] Guid roomId, [FromBody] AddDeviceToRoomDto addDeviceToRoomDto)
        {
            if (_roomService.GetRoomById(roomId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            if (_deviceService.GetDeviceById(addDeviceToRoomDto.deviceId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            try
            {
                var newDevice = _roomService.AddDevice(roomId, addDeviceToRoomDto.deviceId);
                var locationUri = $"{Request.Host}/{Request.Path}/{newDevice.Id}";
                return Created(locationUri, newDevice.FromDeviceToDeviceDto());
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
