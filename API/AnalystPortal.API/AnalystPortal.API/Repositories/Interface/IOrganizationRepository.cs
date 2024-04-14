using AnalystPortal.API.Models.Domain;

namespace AnalystPortal.API.Repositories.Interface
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateAsync(Organization organization);
        Task<IEnumerable<Organization>> GetAllAsync();
        Task<Organization?> GetByIdAsync(int id);
        Task<Organization?> GetByNameAsync(string name);
        Task<Organization?> UpdateAsync(Organization organization);
        Task<Organization?> DeleteAsync(int id);
    }
}
