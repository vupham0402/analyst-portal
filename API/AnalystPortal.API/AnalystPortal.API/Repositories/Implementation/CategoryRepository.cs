using AnalystPortal.API.Data;
using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnalystPortal.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.Include(x => x.Sales).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await dbContext.Categories.Include(x => x.Sales).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCat = await dbContext.Categories.Include(x => x.Sales).FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCat == null)
            {
                return null;
            }

            // Update Organization
            dbContext.Entry(existingCat).CurrentValues.SetValues(category);

            // Update Sales
            existingCat.Sales = category.Sales;

            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var existingCat = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCat != null)
            {
                dbContext.Categories.Remove(existingCat);
                await dbContext.SaveChangesAsync();
                return existingCat;
            }

            return null;
        }
    }
}
