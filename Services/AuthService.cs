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
        private readonly IPendingUserRepository _pendingUserRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IPendingUserRepository pendingUserRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _pendingUserRepository = pendingUserRepository;
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
            return (GetUserByUsername(username) != null && GetPendingUserByUsername(username) != null);
        }

        public User? CreateUser(CreateUserDto user)
        {
            if(_userRepository.GetByUsername(user.Username) != null)
            {
                return null;
            }

            if (_userRepository.GetByEmail(user.Email) != null)
            {
                return null;
            }

            var userRole = UserRoleType.Admin;
            if(user.Role == "SuperAdmin") userRole = UserRoleType.SuperAdmin;
            
            return _userRepository.Create(new User
            {
                Username = user.Username,
                Email = user.Email,
                Role = userRole,
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
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role.ToString()),
                new Claim("email", user.Email),
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

        public PendingUser? GetPendingUserById(Guid id)
        {
            return _pendingUserRepository.GetById(id);
        }

        public PendingUser? GetPendingUserByEmail(string email)
        {
            return _pendingUserRepository.GetByEmail(email);
        }

        public PendingUser? GetPendingUserByUsername(string username)
        {
            return _pendingUserRepository.GetByUsername(username);
        }

        public List<PendingUser> GetPendingUsers(Guid? cursorId = null, bool next = true, int? limit = null, string? keyword = null)
        {
            return _pendingUserRepository.Get(cursorId, next, limit, keyword);
        }

        public PendingUser? CreatePendingUser(CreateUserDto createUserDto)
        {
            if (_userRepository.GetByUsername(createUserDto.Username) != null
                || _pendingUserRepository.GetByUsername(createUserDto.Username) != null)
            {
                return null;
            }

            if (_userRepository.GetByEmail(createUserDto.Email) != null)
            {
                return null;
            }

            var userRole = UserRoleType.Admin;
            if (createUserDto.Role == "SuperAdmin") userRole = UserRoleType.SuperAdmin;

            return _pendingUserRepository.Create(new PendingUser
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Role = userRole,
                Password = HashedPassword(createUserDto.Password)
            });
        }

        public PendingUser? RemovePendingUser(PendingUser pendingUser)
        {
            return _pendingUserRepository.Remove(pendingUser);
        }

        public User? VerifyPendingUser(PendingUser pendingUser)
        {
            if (_userRepository.GetByUsername(pendingUser.Username) != null)
            {
                return null;
            }

            if (_userRepository.GetByEmail(pendingUser.Email) != null)
            {
                return null;
            }

            try
            {
                var newUser = _userRepository.Create(new User
                {
                    Email = pendingUser.Email,
                    Username = pendingUser.Username,
                    Password = pendingUser.Password,
                    Role = pendingUser.Role,
                });

                
                while(true)
                {
                    var user = _pendingUserRepository.GetByEmail(pendingUser.Email);
                    if (user == null) break;

                    _pendingUserRepository.Remove(user);
                }
                return newUser;
            } catch (Exception)
            {
                return null;
            }
            
        }

        public User? GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }
    }
}
