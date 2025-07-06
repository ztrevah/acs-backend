using SystemBackend.Models.DTO;
using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface ICivilianService
    {
        public Civilian? GetCivilianById(String id);
        public List<Civilian> GetCivilians(String? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public Civilian? AddCivilian(AddCivilianDto addCivilianDto);
        public Civilian? UpdateCivilian(String id, UpdateCivilianDto updateCivilianDto);
        public List<Room> GetAccessibleRooms(String civilianId, Guid? roomCursorId = null, bool next = true, int? limit = null, string? keyword = null);
    }
}
