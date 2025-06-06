using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Mappers
{
    public static class CivilianMapper
    {
        public static CivilianDto FromCivilianToCivilianDto(this Civilian civilian)
        {
            return new CivilianDto
            {
                Id = civilian.Id,
                Name = civilian.Name,
                DateOfBirth = civilian.DateOfBirth,
                Hometown = civilian.Hometown,
                ImageUrl = civilian.ImageUrl,
                Email = civilian.Email,
                PhoneNumber = civilian.PhoneNumber,
            };
        }
    }
}
