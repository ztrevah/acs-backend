using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface ICivilianRepository
    {
        public Civilian? GetById(string id);
        public List<Civilian> Get(string? cursorId = null, bool next = true, int? limit = null, string? keyword = null);
        public Civilian Add(Civilian civilian);
        public Civilian? Update(string id, Civilian civilian);
        public List<Room> GetAccessibleRooms(string civilianId, Guid? roomCursorId = null, bool next = true, int? limit = null, string? keyword = null);
    }
}
