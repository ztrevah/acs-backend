using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IImageService
    {
        public Boolean IsImageExisted(String filename);
        public String GetImagePath(String filename);
        public String? UploadImage(IFormFile imageFile);
    }
}
