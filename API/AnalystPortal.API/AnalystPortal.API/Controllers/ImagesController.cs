using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Models.DTO;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Reflection;

namespace AnalystPortal.API.Controllers
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

        // Post: {apiBaseUrl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
                                                     [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                // File Upload
                var orgLogo = new OrganizationLogo
                {
                    FileName = file.FileName,
                    Title = title,
                };
                orgLogo = await imageRepository.Upload(file, orgLogo);

                // Convert Domain Model to DTO
                var response = new OrganizationLogoDto
                {
                    FileName = orgLogo.FileName,
                    Title = orgLogo.Title,
                    Url = orgLogo.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }

        // Get: {apiBaseUrl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            // call image respository to get all images
            var images = await imageRepository.GetAll();

            // Convert Domain model to DTO
            var response = new List<OrganizationLogoDto>();
            foreach (var image in images)
            {
                response.Add(new OrganizationLogoDto
                {
                    FileName = image.FileName,
                    Title = image.Title,
                    Url = image.Url
                });
            }

            return Ok(response);    
        }
    }
}
