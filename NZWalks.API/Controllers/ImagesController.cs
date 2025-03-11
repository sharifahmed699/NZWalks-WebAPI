using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]

        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto dto)
        {
            ValidateFileUpload(dto);

            if(ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = dto.File,
                    FileExtension = Path.GetExtension(dto.File.FileName),
                    FileSizeInBytes = dto.File.Length,
                    FileName = dto.File.FileName,
                    FileDescription = dto.FileDescription

                };

                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto dto)
        {
            var allowedFileExtensions=new string[]{ ".jpg",".jpeg",".png"};

            if(!allowedFileExtensions.Contains(Path.GetExtension(dto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if (dto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
