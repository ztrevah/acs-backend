using SystemBackend.Mappers;
using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;
using SystemBackend.Repositories.Interfaces;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class CivilianService : ICivilianService
    {
        private readonly ICivilianRepository _civilianRepository;
        private readonly IRoomMemberRepository _roomMemberRepository;
        private readonly IImageService _imageService;
        public CivilianService(ICivilianRepository civilianRepository, IRoomMemberRepository roomMemberRepository, IImageService imageService)
        {
            _civilianRepository = civilianRepository;
            _roomMemberRepository = roomMemberRepository;
            _imageService = imageService;
        }

        public Civilian? GetCivilianById(string id)
        {
            return _civilianRepository.GetById(id);
        }

        public List<Civilian> GetCivilians(string? cursorId = null, bool next = true, int? limit = null)
        {
            return _civilianRepository.Get(cursorId, next, limit);
        }

        public List<RoomMember> GetRoomMembers(string civilianId, Guid? roomMemberCursorId = null, bool next = true, int? limit = null)
        {
            if(_civilianRepository.GetById(civilianId) == null)
            {
                return [];
            }

            return _roomMemberRepository.GetByMemberId(civilianId, roomMemberCursorId, next, limit);
        }

        public List<Room> GetAccessibleRooms(String civilianId, Guid? roomCursorId = null, bool next = true, int? limit = null)
        {
            if (_civilianRepository.GetById(civilianId) == null)
            {
                return [];
            }

            return _civilianRepository.GetAccessibleRooms(civilianId, roomCursorId, next, limit);
        }
        public Civilian? AddCivilian(AddCivilianDto addCivilianDto)
        {
            if (_civilianRepository.GetById(addCivilianDto.Id) != null)
            {
                return null;
            }

            try
            {
                var newCivilian = new Civilian
                {
                    Id = addCivilianDto.Id,
                    Name = addCivilianDto.Name,
                    DateOfBirth = addCivilianDto.DateOfBirth,
                    Hometown = addCivilianDto.Hometown,
                    Email = addCivilianDto.Email,
                    PhoneNumber = addCivilianDto.PhoneNumber
                };

                if (addCivilianDto.Image != null)
                {
                    var uploadImage = _imageService.UploadImage(addCivilianDto.Image);
                    newCivilian.ImageUrl = $"/api/images/{uploadImage}";
                }

                return _civilianRepository.Add(newCivilian);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        public Civilian? UpdateCivilian(string id, UpdateCivilianDto updateCivilianDto)
        {
            var exisitingCivilian = _civilianRepository.GetById(id);
            if(exisitingCivilian == null)
            {
                return null;
            }

            try
            {
                exisitingCivilian.Name = updateCivilianDto.Name ?? exisitingCivilian.Name;
                exisitingCivilian.DateOfBirth = updateCivilianDto.DateOfBirth ?? exisitingCivilian.DateOfBirth;
                exisitingCivilian.Hometown = updateCivilianDto.Hometown ?? exisitingCivilian.Hometown;
                exisitingCivilian.Email = updateCivilianDto.Email ?? exisitingCivilian.Email;
                exisitingCivilian.PhoneNumber = updateCivilianDto.PhoneNumber ?? exisitingCivilian.PhoneNumber;

                if (updateCivilianDto.Image != null)
                {
                    var uploadImage = _imageService.UploadImage(updateCivilianDto.Image);
                    exisitingCivilian.ImageUrl = $"/api/images/{uploadImage}";
                }
                return _civilianRepository.Update(id, exisitingCivilian);
            }
            catch(Exception)
            {
                return null;
            }            
        }
    }
}
