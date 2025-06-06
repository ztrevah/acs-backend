using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
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
        public IActionResult GetAllCivilians(string? cursorId = null, bool next = true, int limit = 20)
        {
            var civilians = _civilianService.GetCivilians(cursorId, next, limit)
                .Select(c => c.FromCivilianToCivilianDto())
                .ToList();
            Console.WriteLine(civilians);
            return Ok(new
            {
                cursorId = civilians.Count == 0 ? null : civilians.Last().Id,
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
        public IActionResult GetAccessibleRooms([FromRoute] string civilianId, Guid? cursorId = null, bool next = true, int limit = 20)
        {
            var civilian = _civilianService.GetCivilianById(civilianId);
            if(civilian == null)
            {
                return NotFound(new
                {
                    error = new { message = "Civilian not found." }
                });
            }

            var accessibleRooms = _civilianService.GetAccessibleRooms(civilianId, cursorId, next, limit);

            return Ok(new
            {
                cursorId = accessibleRooms.Count == 0 ? (Guid?) null : accessibleRooms.Last().Id,
                count = accessibleRooms.Count,
                data = accessibleRooms
            });
        }
    }
}
