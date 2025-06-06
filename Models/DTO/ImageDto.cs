namespace SystemBackend.Models.DTO
{
    public class UploadImageDto
    {
        public required IFormFile ImageFile { get; set; }
    }

    public class UploadImageResponseDto
    {
        public required string ImageUrl { get; set; }
    }
}
