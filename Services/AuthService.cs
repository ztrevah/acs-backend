using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }

        public User? GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User? GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }
        public bool CheckExistingUsername(string username)
        {
            return (GetUserByUsername(username) != null);
        }

        public User? CreateUser(CreateUserDto user)
        {
            if(_userRepository.GetByUsername(user.Username) != null)
            {
                return null;
            }

            return _userRepository.Create(new User
            {
                Username = user.Username,
                Password = HashedPassword(user.Password)
            });
        }

        public string GenerateAccessToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
            };

            var accessTokenValidDuration = jwtSettings.GetValue<int>("AccessTokenValidDuration");

            var token = new JwtSecurityToken(
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                claims,
                expires: DateTime.UtcNow.AddSeconds(accessTokenValidDuration),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken? GenerateRefreshToken(User user)
        {
            if (GetUserById(user.Id) == null) { return null; }

            try
            {
                return _refreshTokenRepository.Create(new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("Jwt:RefreshTokenValidDuration")),
                    UserId = user.Id,
                });
            }
            catch(Exception)
            {
                return null;
            }
        }

        public RefreshToken? GetRefreshToken(string refreshToken)
        {
            return _refreshTokenRepository.Get(refreshToken);
        }
        

        public string HashedPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool CheckPassword(string rawPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hashedPassword);
        }

        public RefreshToken? RemoveRefreshToken(string refreshToken)
        {
            return _refreshTokenRepository.Remove(refreshToken);
        }
    }
}
