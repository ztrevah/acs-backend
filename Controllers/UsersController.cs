using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        public UsersController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }
        [HttpGet("pending")]
        public IActionResult GetPendingUsers(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            if (limit < 0)
            {
                return BadRequest(new
                {
                    error = new { message = "limit should be a non-negative integer." }
                });
            }

            var pendingUsers = _authService.GetPendingUsers(cursorId, next, limit + 1, keyword)
                .Select(u => u.FromPendingUserToPendingUserDto())
                .ToList();

            var nextId = (pendingUsers.Count == limit + 1) ? (Guid?) pendingUsers.Last().Id : null;
            if (pendingUsers.Count == limit + 1) pendingUsers.Remove(pendingUsers.Last());
            return Ok(new
            {
                cursorId = nextId,
                count = pendingUsers.Count,
                data = pendingUsers
            });
        }

        [HttpGet("pending/{id}")]
        public IActionResult GetPendingUserById(Guid id)
        {
            var pendingUser = _authService.GetPendingUserById(id);

            if(pendingUser == null)
            {
                return NotFound(new
                {
                    error = new { message = "Pending user not found." }
                });
            }

            return Ok(pendingUser.FromPendingUserToPendingUserDto());
        }

        [HttpPost("")]
        public IActionResult VerifyPendingUser([FromBody] VerifyPendingUserDto verifyPendingUserDto)
        {
            var pendingUser = _authService.GetPendingUserById(verifyPendingUserDto.Id);

            if (pendingUser == null)
            {
                return NotFound(new
                {
                    error = new { message = "Pending user not found." }
                });
            }

            try
            {
                var newUser = _authService.VerifyPendingUser(pendingUser);

                if (newUser == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                try
                {
                    _emailService.SendApproveCreateAccountRequest(pendingUser);
                    return Ok(new UserDto
                    {
                        Id = newUser.Id,
                        Username = newUser.Username,
                        Email = newUser.Email,
                        Role = newUser.Role.ToString(),
                    });
                } 
                catch(Exception)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }
                
            } catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
            
        }

        [HttpDelete("pending/{id}")]
        public IActionResult RemovePendingUser(Guid id)
        {
            var pendingUser = _authService.GetPendingUserById(id);

            if (pendingUser == null)
            {
                return NotFound(new
                {
                    error = new { message = "Pending user not found." }
                });
            }

            try
            {
                var removePendingUser = _authService.RemovePendingUser(pendingUser);

                if (removePendingUser == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                try
                {
                    _emailService.SendDenyCreateAccountRequest(removePendingUser);
                    return Ok(removePendingUser.FromPendingUserToPendingUserDto());
                }
                catch (Exception)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }
               
            } catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
            
        }
    }
}
