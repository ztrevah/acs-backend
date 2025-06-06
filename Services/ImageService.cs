using System.Net.Http.Headers;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly String _uploadDirPath = "uploads";
        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            if (!Directory.Exists(_uploadDirPath))
            {
                Directory.CreateDirectory(_uploadDirPath);
            }
        }
        public String? UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            if (!imageFile.ContentType.StartsWith("image/"))
            {
                return null;
            }

            try
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(imageFile.ContentDisposition).FileName;
                if(originalFileName == null)
                {
                    return null;
                }

                originalFileName = originalFileName.Trim('"');
                var fileExtension = Path.GetExtension(originalFileName);

                var newImageFilename = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(_uploadDirPath, newImageFilename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return $"{newImageFilename}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsImageExisted(string filename)
        {
            return System.IO.File.Exists(GetImagePath(filename));
        }

        public string GetImagePath(string filename)
        {
            return Path.Combine(_uploadDirPath, filename);
        }
    }
}
