using AnalystPortal.API.Models.Domain;

namespace AnalystPortal.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<OrganizationLogo> Upload(IFormFile file, OrganizationLogo organizationLogo);
        Task<IEnumerable<OrganizationLogo>> GetAll();
    }
}
