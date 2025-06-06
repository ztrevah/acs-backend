using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SystemBackend.Models.DTO;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/images")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpGet("{filename}")]
        public IActionResult GetImage([FromRoute] string filename)
        {
            if(_imageService.IsImageExisted(filename))
            {
                string imagePath = _imageService.GetImagePath(filename);
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(imagePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                return File(imageBytes, contentType);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm] UploadImageDto uploadImageDto)
        {
            if (uploadImageDto.ImageFile == null || uploadImageDto.ImageFile.Length == 0)
            {
                return BadRequest(new
                {
                    error = new { message = "No image uploaded." }
                });
            }

            if (!uploadImageDto.ImageFile.ContentType.StartsWith("image/"))
            {
                return BadRequest(new
                {
                    error = new { message = "Only image file is allowed." }
                });
            }

            try
            {
                var newImageFilename = _imageService.UploadImage(uploadImageDto.ImageFile);
                if(newImageFilename == null)
                {
                    return StatusCode(500, new
                    {
                        error = new { message = "Internal server error." }
                    });
                }

                return CreatedAtAction(
                    nameof(GetImage),
                    new { filename = newImageFilename },
                    new UploadImageResponseDto
                    {
                        ImageUrl = $"/api/images/{newImageFilename}"
                    }
                );
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new { message = $"Internal server error, {ex.Message}" }
                });
            }
        }
    }
}
