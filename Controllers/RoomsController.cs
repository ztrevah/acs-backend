using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
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
        public IActionResult GetAllRooms(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var rooms = _roomService.GetRooms(cursorId, next, limit + 1, keyword)
                .Select(r => r.FromRoomToRoomDto())
                .ToList();

            var nextId = (rooms.Count == limit + 1) ?  rooms.Last().Id : (Guid?)null;
            if(rooms.Count == limit + 1) rooms.Remove(rooms.Last());
            return Ok(new
            {
                cursorId = nextId,
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
        public IActionResult GetRoomMembers([FromRoute] Guid roomId, string? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool onlyAllowed = false)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var room = _roomService.GetRoomById(roomId);
            if(room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var members = _roomService.GetMembers(roomId, cursorId, next, limit + 1, keyword, onlyAllowed)
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.DateOfBirth,
                    m.Hometown,
                    m.ImageUrl,
                    m.Email,
                    m.PhoneNumber,
                    Status = _roomService.GetRoomMemberStatus(roomId, m.Id).FromAccessStatusToString(),
                })
                .ToList();

            var nextId = (members.Count == limit + 1) ? members.Last().Id : null;
            if (members.Count == limit + 1) members.Remove(members.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = members.Count,
                data = members
            });
        }

        [HttpGet("{roomId}/members/{civilianId}")]
        public IActionResult GetRoomMemberInfo([FromRoute] Guid roomId, [FromRoute] string civilianId)
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

            var rm = _roomService.GetMemberRights(roomId, civilianId)
                .Select(rm => rm.FromRoomMemberToRoomMemberDto())
                .ToList();

            return Ok(new
            {
                RoomId = roomId,
                MemberId = civilianId,
                Status = _roomService.GetRoomMemberStatus(roomId, civilianId).FromAccessStatusToString(),
                AccessRights = rm
            });
        }

        [HttpPost("{roomId}/members/{civilianId}/rights")]
        public IActionResult AddRoomMemberRights([FromRoute] Guid roomId, [FromRoute] string civilianId, [FromBody] AddRoomMemberDto addRoomMemberDto)
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

            if((addRoomMemberDto.DisabledEndTime == null && addRoomMemberDto.DisabledStartTime != null) 
                || (addRoomMemberDto.DisabledEndTime != null && addRoomMemberDto.DisabledStartTime == null))
            {
                return BadRequest(new
                {
                    error = new { message = "Start time and end time of disabled period must be both null or both not null." }
                });
            }

            if(addRoomMemberDto.StartTime >= addRoomMemberDto.EndTime || addRoomMemberDto.DisabledStartTime >= addRoomMemberDto.DisabledEndTime
                || addRoomMemberDto.DisabledStartTime < addRoomMemberDto.StartTime || addRoomMemberDto.DisabledEndTime > addRoomMemberDto.EndTime)
            {
                return BadRequest(new
                {
                    error = new { message = "Invalid access period." }
                });
            }

            try
            {
                var newRoomMember = _roomService.AddRoomMember(roomId, civilianId, addRoomMemberDto);
                if(newRoomMember == null)
                {
                    return BadRequest(new
                    {
                        error = new { message = "Conflicted access period." }
                    });
                }

                return CreatedAtAction(nameof(GetRoomMember), new { roomId, civilianId, newRoomMember.Id }, newRoomMember.FromRoomMemberToRoomMemberDto());
            }
            catch(Exception e)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpGet("{roomId}/members/{civilianId}/rights/{id}")]
        public IActionResult GetRoomMember([FromRoute] Guid roomId, [FromRoute] string civilianId, [FromRoute] Guid id)
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

            var roomMember = _roomService.GetRoomMember(id, roomId, civilianId);
            if (roomMember == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room member's access right not found." }
                });
            }

            return Ok(roomMember.FromRoomMemberToDetailResponselDto());
        }

        [HttpPut("{roomId}/members/{civilianId}/rights/{id}")]
        public IActionResult UpdateRoomMemberRight([FromRoute] Guid roomId, [FromRoute] string civilianId, [FromRoute] Guid id, [FromBody] UpdateRoomMemberDto updateRoomMemberDto)
        {
            if (_roomService.GetRoomMember(id, roomId, civilianId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room member's access right not found." }
                });
            }

            if ((updateRoomMemberDto.DisabledEndTime == null && updateRoomMemberDto.DisabledStartTime != null)
                || (updateRoomMemberDto.DisabledEndTime != null && updateRoomMemberDto.DisabledStartTime == null))
            {
                return BadRequest(new
                {
                    error = new { message = "Start time and end time of disabled period must be both null or both not null." }
                });
            }

            if (updateRoomMemberDto.StartTime > updateRoomMemberDto.EndTime || updateRoomMemberDto.DisabledStartTime > updateRoomMemberDto.DisabledEndTime
                || updateRoomMemberDto.DisabledStartTime < updateRoomMemberDto.StartTime || updateRoomMemberDto.DisabledEndTime > updateRoomMemberDto.EndTime)
            {
                return BadRequest(new
                {
                    error = new { message = "Invalid access time." }
                });
            }

            try
            {
                var newRoomMember = _roomService.UpdateRoomMember(id, updateRoomMemberDto);
                if (newRoomMember == null)
                {
                    return StatusCode(400, new
                    {
                        error = new { message = "Conflicted access period." }
                    });
                }

                return Ok(newRoomMember.FromRoomMemberToRoomMemberDto());
            }
            catch (Exception)
            {
                return BadRequest(new
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
                var numRemovedRights = _roomService.RemoveAllMemberRights(roomId, civilianId);

                return Ok(new
                {
                    RoomId = roomId,
                    MemberId = civilianId,
                    count = numRemovedRights
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
        }

        [HttpDelete("{roomId}/members/{civilianId}/rights/{id}")]
        public IActionResult RemoveMemberRight([FromRoute] Guid roomId, [FromRoute] string civilianId, [FromRoute] Guid id)
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

            if (_roomService.GetRoomMember(id, roomId, civilianId) == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room member's access right not found." }
                });
            }

            try
            {
                var removedRight = _roomService.RemoveRoomMember(id);
                if (removedRight == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                return Ok(removedRight.FromRoomMemberToDetailResponselDto());
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
        public IActionResult GetRoomDevices([FromRoute] Guid roomId, Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null, bool? isIn = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound(new
                {
                    error = new { message = "Room not found." }
                });
            }

            var devices = _roomService.GetDevices(roomId, cursorId, next, limit + 1, keyword, isIn)
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
                    error = new { message = "Device not found." }
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
