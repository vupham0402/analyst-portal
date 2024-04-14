using AnalystPortal.API.Data;
using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Repositories.Interface;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;

namespace AnalystPortal.API.Repositories.Implementation
{
    // store locally
    /*public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
                               IHttpContextAccessor httpContextAccessor,
                               ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<OrganizationLogo>> GetAll()
        {
            return await dbContext.OrganizationLogos.ToListAsync();
        }

        public async Task<OrganizationLogo> Upload(IFormFile file, OrganizationLogo organizationLogo)
        {
            // Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath,
                                         "Images", $"{organizationLogo.FileName}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Update the database
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{organizationLogo.FileName}";
            organizationLogo.Url = urlPath;
            await dbContext.OrganizationLogos.AddAsync(organizationLogo);
            await dbContext.SaveChangesAsync();

            return organizationLogo;
        }
    }*/

    // store on azure blob storage
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName = "analystportallogos"; // Name of your Azure Blob Container

        public ImageRepository(ApplicationDbContext dbContext,
                               BlobServiceClient blobServiceClient)
        {
            this.dbContext = dbContext;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<IEnumerable<OrganizationLogo>> GetAll()
        {
            return await dbContext.OrganizationLogos.ToListAsync();
        }

        public async Task<OrganizationLogo> Upload(IFormFile file, OrganizationLogo organizationLogo)
        {
            var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainer.GetBlobClient(organizationLogo.FileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            // Update the database
            var urlPath = blobClient.Uri.AbsoluteUri;
            organizationLogo.Url = urlPath;
            await dbContext.OrganizationLogos.AddAsync(organizationLogo);
            await dbContext.SaveChangesAsync();

            return organizationLogo;
        }
    }
}
