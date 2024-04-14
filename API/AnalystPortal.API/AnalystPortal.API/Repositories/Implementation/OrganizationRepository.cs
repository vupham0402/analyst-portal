using AnalystPortal.API.Data;
using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnalystPortal.API.Repositories.Implementation
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrganizationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Organization> CreateAsync(Organization organization)
        {
            await dbContext.Organizations.AddAsync(organization);
            await dbContext.SaveChangesAsync();
            return organization;
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await dbContext.Organizations.Include(x => x.Sales).ToListAsync();
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            return await dbContext.Organizations.Include(x => x.Sales).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Organization?> UpdateAsync(Organization organization)
        {
            var existingOrg = await dbContext.Organizations.Include(x => x.Sales).FirstOrDefaultAsync(x => x.Id == organization.Id);
            if (existingOrg == null)
            {
                return null;
            }

            // Update Organization
            dbContext.Entry(existingOrg).CurrentValues.SetValues(organization);

            // Update Sales
            existingOrg.Sales = organization.Sales;

            await dbContext.SaveChangesAsync();

            return organization;
        }

        public async Task<Organization?> DeleteAsync(int id)
        {
            var existingOrg = await dbContext.Organizations.FirstOrDefaultAsync(x => x.Id == id);
            if (existingOrg != null)
            {
                dbContext.Organizations.Remove(existingOrg);
                await dbContext.SaveChangesAsync();
                return existingOrg;
            }

            return null;
        }

        public async Task<Organization?> GetByNameAsync(string name)
        {
            return await dbContext.Organizations.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
