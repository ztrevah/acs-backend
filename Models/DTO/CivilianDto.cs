namespace SystemBackend.Models.DTO
{
    public class CivilianDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Hometown { get; set; }
        public string? ImageUrl { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AddCivilianDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Hometown { get; set; }
        public IFormFile? Image { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UpdateCivilianDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Hometown { get; set; }
        public IFormFile? Image { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AddCivilianToRoomDto
    {
        public required string civilianId { get; set; }
    }
}
