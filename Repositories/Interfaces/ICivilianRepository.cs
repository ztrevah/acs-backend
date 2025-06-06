using SystemBackend.Models.Entities;

namespace SystemBackend.Repositories.Interfaces
{
    public interface ICivilianRepository
    {
        public Civilian? GetById(String id);
        public List<Civilian> Get(String? cursorId = null, bool next = true, int limit = 20);
        public Civilian Add(Civilian civilian);
        public Civilian? Update(String id, Civilian civilian);
        public List<Room> GetAccessibleRooms(String civilianId, Guid? roomCursorId = null, bool next = true, int limit = 20);
    }
}
