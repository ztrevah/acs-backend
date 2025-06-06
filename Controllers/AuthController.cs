using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SystemBackend.Data;
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
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] CreateUserDto createUserDto)
        {
            if(_authService.CheckExistingUsername(createUserDto.Username))
            {
                return BadRequest(new
                {
                    error = new { message = "This username has already existed!" }
                });
            }

            try
            {
                var newUser = _authService.CreateUser(createUserDto);
                if(newUser == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }
                
                return StatusCode(201, new SuccessfulCreateUserDto
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
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

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInUserDto signInUserDto)
        {
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
