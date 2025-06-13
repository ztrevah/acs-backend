using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using SystemBackend.Data;
using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IEmailService emailService, IConfiguration configuration)
        {
            _authService = authService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] CreateUserDto createUserDto)
        {
            if(_authService.GetUserByUsername(createUserDto.Username) != null 
                || _authService.GetPendingUserByUsername(createUserDto.Username) != null)
            {
                return BadRequest(new
                {
                    error = new { message = "This username has already existed!" }
                });
            }

            if (_authService.GetUserByEmail(createUserDto.Email) != null)
            {
                return BadRequest(new
                {
                    error = new { message = "This email has already existed!" }
                });
            }

            if(createUserDto.Password.Length < 8)
            {
                return BadRequest(new
                {
                    error = new { message = "The password must contain more than 8 characters!" }
                });
            }

            string emailRegexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            Regex regex = new Regex(emailRegexPattern);
            if (!regex.IsMatch(createUserDto.Email))
            {
                return BadRequest(new
                {
                    error = new { message = "The email is invalid!" }
                });
            }

            try
            {
                var newUser = _authService.CreatePendingUser(createUserDto);
                if(newUser == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }


                try
                {
                    _emailService.SendReceiveCreateAccountRequest(newUser);
                    return StatusCode(200, newUser.FromPendingUserToPendingUserDto());
                }
                catch (Exception ex)
                {
                    _authService.RemovePendingUser(newUser);
                    return StatusCode(500, new
                    {
                        error = new { message = "Error sending email." }
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error." }
                });
            }
            
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInUserDto signInUserDto)
        {
            if(_authService.GetPendingUserByUsername(signInUserDto.Username) != null)
            {
                return BadRequest(new
                {
                    error = new { message = "This user is pending for verification." }
                });
            }

            var signInUser = _authService.GetUserByUsername(signInUserDto.Username);
            if(signInUser == null)
            {
                return NotFound(new
                {
                    error = new { message = "User not found." }
                });
            }

            if(!_authService.CheckPassword(signInUserDto.Password, signInUser.Password))
            {
                return Unauthorized(new
                {
                    error = new { message = "Wrong password." }
                });
            }

            try
            {
                var refreshToken = _authService.GenerateRefreshToken(signInUser);
                if (refreshToken == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenValidDuration")),
                    IsEssential = true,
                    Path = "/"
                });
                return Ok(new SuccessfulSignInUserDto
                {
                    Id = signInUser.Id,
                    Username = signInUser.Username,
                    Email = signInUser.Email,
                    Role = signInUser.Role.ToString(),
                    AccessToken = _authService.GenerateAccessToken(signInUser)
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

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            string? refreshTokenString = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshTokenString))
            {
                if(refreshTokenString != null) _authService.RemoveRefreshToken(refreshTokenString);
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/" });
                return Unauthorized(new
                {
                    error = new { message = "Refresh token not found." }
                });
            }

            var refreshToken = _authService.GetRefreshToken(refreshTokenString);
            if(refreshToken == null)
            {
                _authService.RemoveRefreshToken(refreshTokenString);
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/" });
                return Unauthorized(new
                {
                    error = new { message = "Invalid refresh token." }
                });
            }

            var user = _authService.GetUserById(refreshToken.UserId);
            if (user == null)
            {
                _authService.RemoveRefreshToken(refreshTokenString);
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/" });
                return Unauthorized(new
                {
                    error = new { message = "Invalid refresh token." }
                });
            }

            try
            {
                return Ok(new { accessToken = _authService.GenerateAccessToken(user) });
            }
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error" }
                });
            }
            
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            string? refreshTokenString = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshTokenString))
            {
                Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/" });
                return Unauthorized(new {
                    error = new { message = "Refresh token not found." }
                });
            }

            _authService.RemoveRefreshToken(refreshTokenString);
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/" });
            return Ok();
        }
    }
}
