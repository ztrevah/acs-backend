using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/civilians")]
    [ApiController]
    [Authorize]
    public class CiviliansController : ControllerBase
    {
        private readonly ICivilianService _civilianService;
        public CiviliansController(ICivilianService civilianService)
        {
            _civilianService = civilianService;
        }

        [HttpGet("")]
        public IActionResult GetAllCivilians(string? cursorId = null, bool next = true, int? limit = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var civilians = _civilianService.GetCivilians(cursorId, next, limit + 1)
                .Select(c => c.FromCivilianToCivilianDto())
                .ToList();

            var nextId = (civilians.Count == limit + 1) ? civilians.Last().Id : null;
            if (civilians.Count == limit + 1) civilians.Remove(civilians.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = civilians.Count,
                data = civilians
            });
        }

        [HttpGet("{civilianId}")]
        public IActionResult GetCivilianById([FromRoute] string civilianId)
        {
            var civilian = _civilianService.GetCivilianById(civilianId);
            if(civilian ==  null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }
            return Ok(civilian.FromCivilianToCivilianDto());
        }

        [HttpPut("{civilianId}")]
        public IActionResult UpdateCivilianById([FromRoute] string civilianId, [FromForm] UpdateCivilianDto updateCivilianDto)
        {
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
                var updateCivilian = _civilianService.UpdateCivilian(civilianId, updateCivilianDto);
                if(updateCivilian == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error" }
                    });
                }

                return Ok(updateCivilian.FromCivilianToCivilianDto());
            } catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error" }
                }); 
            }
            
        }

        [HttpPost("")]
        public IActionResult AddCivilian([FromForm] AddCivilianDto addCivilianDto)
        {
            var civilian = _civilianService.GetCivilianById(addCivilianDto.Id);
            if (civilian != null)
            {
                return BadRequest(new
                {
                    error = new { message = "This civilian has already existed." }
                });
            }

            try
            {
                var addCivilian = _civilianService.AddCivilian(addCivilianDto);
                if (addCivilian == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error" }
                    });
                }

                return CreatedAtAction(
                    nameof(GetCivilianById),
                    new { civilianId =  addCivilian.Id },
                    addCivilian.FromCivilianToCivilianDto()
                );
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error" }
                });
            }
        }

        [HttpGet("{civilianId}/rooms")]
        public IActionResult GetAccessibleRooms([FromRoute] string civilianId, Guid? cursorId = null, bool next = true, int? limit = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var civilian = _civilianService.GetCivilianById(civilianId);
            if(civilian == null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }

            var accessibleRooms = _civilianService.GetAccessibleRooms(civilianId, cursorId, next, limit + 1)
                .Select(r => r.FromRoomToRoomDto())
                .ToList();

            var nextId = (accessibleRooms.Count == limit + 1) ? accessibleRooms.Last().Id : (Guid?)null;
            if (accessibleRooms.Count == limit + 1) accessibleRooms.Remove(accessibleRooms.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = accessibleRooms.Count,
                data = accessibleRooms
            });
        }
    }
}
