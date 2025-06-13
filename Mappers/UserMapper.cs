using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class UserMapper
    {
        public static PendingUserDto FromPendingUserToPendingUserDto (this PendingUser pendingUser)
        {
            return new PendingUserDto
            {
                Id = pendingUser.Id,
                Username = pendingUser.Username,
                Email = pendingUser.Email,
                Role = pendingUser.Role.ToString(),
            };
        }
    }
}
