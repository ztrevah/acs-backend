using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class UserMapper
    {
        public static User FromCreateUserDtoToAdmin(this CreateUserDto createUserDto)
        {
            return new User
            {
                Username = createUserDto.Username,
                Password = createUserDto.Password,
            };
        }
    }
}
